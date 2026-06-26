using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public sealed class GetProfilesForAdminsQueryHandler : IRequestHandler<GetProfilesForAdminsQuery, GetProfilesForAdminsResponse>
    {
        private readonly IRafeekDbContext _context;
        private readonly IIdentityUnitOfWork _identityUnitOfWork;

        public GetProfilesForAdminsQueryHandler(IRafeekDbContext context, IIdentityUnitOfWork identityUnitOfWork)
        {
            _context = context;
            _identityUnitOfWork = identityUnitOfWork;
        }

        public async Task<GetProfilesForAdminsResponse> Handle(GetProfilesForAdminsQuery request, CancellationToken cancellationToken)
        {
            var user = await _identityUnitOfWork.ApplicationUserRepository
                .GetAll()
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);

            if (user == null)
                throw new NotFoundException($"المستخدم بالمعرّف {request.UserId} غير موجود.");

            var allUserTypes = GetFlags(user.UserTypes);
            var effectiveTypes = request.UserTypes?.Count > 0
                ? request.UserTypes.Where(t => allUserTypes.Contains(t)).ToList()
                : allUserTypes;

            var response = new GetProfilesForAdminsResponse
            {
                UserId = user.Id,
                FullName = user.FullName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                ProfilePictureUrl = user.ProfilePictureUrl,
                UserTypes = allUserTypes
            };

            var profileData = await LoadProfileDataAsync(request.UserId, effectiveTypes, cancellationToken);
            response.DepartmentName = profileData.DepartmentName;

            foreach (var type in effectiveTypes)
            {
                if (_loaders.TryGetValue(type, out var loader))
                {
                    await loader(response, profileData, _context, cancellationToken);
                }
            }

            return response;
        }

        private sealed record ProfileData(
            Doctor? Doctor,
            Staff? Staff,
            Student? Student,
            string? DepartmentName
        );

        private async Task<ProfileData> LoadProfileDataAsync(Guid userId, List<UserType> types, CancellationToken ct)
        {
            Doctor? doctor = null;
            Staff? staff = null;
            Student? student = null;

            var needsDoctor = types.Any(t => t is UserType.Mentor or UserType.Professor);
            var needsStaff = types.Contains(UserType.Staff);
            var needsStudent = types.Contains(UserType.Student);

            if (needsDoctor)
            {
                doctor = await _context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.UserId == userId, ct);
            }

            if (needsStaff)
            {
                staff = await _context.Staffs
                    .AsNoTracking()
                    .FirstOrDefaultAsync(s => s.UserId == userId, ct);
            }

            if (needsStudent)
            {
                student = await _context.Students
                    .AsNoTracking()
                    .Include(s => s.Department)
                    .FirstOrDefaultAsync(s => s.UserId == userId, ct);
            }

            return new ProfileData(
                doctor,
                staff,
                student,
                doctor?.Department?.Name ?? student?.Department?.Name
            );
        }

        private delegate Task ProfileLoader(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct);

        private static readonly Dictionary<UserType, ProfileLoader> _loaders = new()
        {
            [UserType.Mentor] = LoadMentorProfileAsync,
            [UserType.Professor] = LoadProfessorProfileAsync,
            [UserType.Student] = LoadStudentProfileAsync,
            [UserType.Staff] = LoadStaffProfileAsync,
            [UserType.Admin] = LoadAdminProfileAsync,
        };

        private static async Task LoadMentorProfileAsync(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct)
        {
            if (profile.Doctor == null)
            {
                response.Mentor = new MentorProfileSection();
                return;
            }

            var (officeHours, teachingCourses) = await LoadTeachingDataAsync(profile.Doctor, context, ct);

            var advisedStudents = await context.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Where(s => s.AcademicAdvisorId == profile.Doctor.Id && !s.IsDeleted)
                .Select(s => new
                {
                    s.User.Id,
                    s.User.FullName,
                    s.User.ProfilePictureUrl,
                    s.UniversityCode,
                    s.Level,
                    s.Status
                })
                .ToListAsync(ct);

            var advisedStudentDtos = advisedStudents.Select(s => new AdvisedStudentDto
            {
                Id = s.Id,
                Name = s.FullName,
                Image = s.ProfilePictureUrl,
                UniversityCode = s.UniversityCode,
                Level = s.Level,
                LevelName = s.Level switch
                {
                    1 => "الفرقة الأولى",
                    2 => "الفرقة الثانية",
                    3 => "الفرقة الثالثة",
                    _ => "الفرقة الرابعة"
                },
                Status = s.Status == Rafeek.Domain.Enums.StudentStatus.Active ? "نشط" : "غير نشط"
            }).ToList();

            response.Mentor = new MentorProfileSection
            {
                AcademicTitle = profile.Doctor.IsAcademicAdvisor ? "أستاذ / مرشد أكاديمي" : "أستاذ دكتور",
                EmployeeNumber = profile.Doctor.EmployeeCode,
                Status = profile.Doctor.IsActive ? "نشط" : "غير نشط",
                OfficeHours = officeHours,
                TeachingCourses = teachingCourses,
                AdvisedStudents = advisedStudentDtos
            };
        }

        private static async Task LoadProfessorProfileAsync(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct)
        {
            if (profile.Doctor == null) return;

            var (officeHours, teachingCourses) = await LoadTeachingDataAsync(profile.Doctor, context, ct);

            response.Professor = new ProfessorProfileSection
            {
                AcademicTitle = profile.Doctor.IsAcademicAdvisor ? "أستاذ / مرشد أكاديمي" : "أستاذ دكتور",
                EmployeeNumber = profile.Doctor.EmployeeCode,
                Status = profile.Doctor.IsActive ? "نشط" : "غير نشط",
                OfficeHours = officeHours,
                TeachingCourses = teachingCourses
            };
        }

        private static async Task<(List<OfficeHourDto> OfficeHours, List<TeachingCourseDto> TeachingCourses)> LoadTeachingDataAsync(
            Doctor doctor, IRafeekDbContext context, CancellationToken ct)
        {
            var sections = await context.LectureGroups
                .AsNoTracking()
                .Include(s => s.Course)
                .Include(s => s.CalendarEvents)
                    .ThenInclude(ce => ce.AcademicTerm)
                        .ThenInclude(at => at!.AcademicYear)
                .Where(s => s.DoctorId == doctor.Id && !s.IsDeleted)
                .ToListAsync(ct);

            var officeHours = sections
                .GroupBy(s => new { s.Day, s.StartTime, s.EndTime })
                .Select(g =>
                {
                    var first = g.First();
                    return new OfficeHourDto
                    {
                        Day = first.Day,
                        StartTime = first.StartTime?.ToString(@"hh\:mm"),
                        EndTime = first.EndTime?.ToString(@"hh\:mm"),
                        Location = first.Location,
                        Capacity = first.Capacity
                    };
                })
                .ToList();

            var teachingCourses = sections
                .GroupBy(s => s.CourseId)
                .Select(g => g.First())
                .Select(s =>
                {
                    var term = s.CalendarEvents?
                        .Select(ce => ce.AcademicTerm)
                        .FirstOrDefault(t => t != null);

                    return new TeachingCourseDto
                    {
                        CourseId = s.Course.Id,
                        CourseCode = s.Course.Code,
                        CourseName = s.Course.Title,
                        CreditHours = s.Course.CreditHours,
                        Semester = term?.Name,
                        AcademicYear = term?.AcademicYear?.Name
                    };
                })
                .ToList();

            return (officeHours, teachingCourses);
        }

        private static async Task LoadStudentProfileAsync(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct)
        {
            if (profile.Student == null) return;

            var student = profile.Student;

            var enrollments = await context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Include(e => e.LectureGroup)
                    .ThenInclude(sec => sec!.Doctor)
                        .ThenInclude(d => d!.User)
                .Include(e => e.LectureGroup)
                    .ThenInclude(sec => sec.CalendarEvents)
                        .ThenInclude(ce => ce.AcademicTerm)
                            .ThenInclude(at => at!.AcademicYear)
                .Include(e => e.Grades)
                .Where(e => e.StudentId == student.Id && !e.IsDeleted)
                .ToListAsync(ct);

            var level = (student.AcademicProfileId != Guid.Empty ? 1 : 0) + student.Level;
            var levelName = level switch
            {
                1 => "الفرقة الأولى",
                2 => "الفرقة الثانية",
                3 => "الفرقة الثالثة",
                4 => "الفرقة الرابعة",
                _ => $"المستوى {level}"
            };

            var advisorName = student.AcademicAdvisorId.HasValue
                ? await context.Doctors
                    .AsNoTracking()
                    .Include(d => d.User)
                    .Where(d => d.Id == student.AcademicAdvisorId)
                    .Select(d => d.User.FullName)
                    .FirstOrDefaultAsync(ct)
                : null;

            var section = new StudentProfileSection
            {
                UniversityCode = student.UniversityCode,
                Level = level,
                LevelName = levelName,
                AcademicAdvisorName = advisorName ?? "-"
            };

            foreach (var enrollment in enrollments)
            {
                var isCompleted = !string.IsNullOrEmpty(enrollment.Grade);

                if (!isCompleted)
                {
                    section.CurrentEnrollments.Add(new AdminStudentCurrentEnrollmentDto
                    {
                        CourseCode = enrollment.Course.Code,
                        CourseTitle = enrollment.Course.Title,
                        InstructorName = enrollment.LectureGroup?.Doctor?.User?.FullName ?? "-",
                        Status = "Enrolled",
                        StatusLabel = "مسجل"
                    });
                }
                else
                {
                    var finalGrade = enrollment.Grades
                        .OrderByDescending(g => g.AbsoluteScore)
                        .FirstOrDefault();

                    var term = enrollment.LectureGroup?.CalendarEvents?
                        .Select(ce => ce.AcademicTerm)
                        .FirstOrDefault(t => t != null);

                    var semesterName = "-";
                    if (term != null)
                    {
                        semesterName = $"{term.Name} {term.AcademicYear?.Name}";
                    }

                    section.ResultsHistory.Add(new AdminStudentResultHistoryDto
                    {
                        CourseCode = enrollment.Course.Code,
                        CourseTitle = enrollment.Course.Title,
                        SemesterName = semesterName,
                        Score = finalGrade?.AbsoluteScore,
                        Grade = enrollment.Grade ?? "N/A"
                    });
                }
            }

            section.ResultsHistory = section.ResultsHistory
                .OrderByDescending(r => r.SemesterName)
                .ThenBy(r => r.CourseCode)
                .ToList();

            response.Student = section;
        }

        private static async Task LoadStaffProfileAsync(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct)
        {
            if (profile.Staff == null && profile.Doctor == null) return;

            var doctor = profile.Doctor;

            if (doctor == null && profile.Staff != null)
            {
                doctor = await context.Doctors
                    .AsNoTracking()
                    .Include(d => d.Department)
                    .FirstOrDefaultAsync(d => d.UserId == response.UserId, ct);
            }

            var sections = doctor != null
                ? await context.LectureGroups
                    .AsNoTracking()
                    .Include(s => s.Course)
                    .Include(s => s.CalendarEvents)
                        .ThenInclude(ce => ce.AcademicTerm)
                            .ThenInclude(at => at!.AcademicYear)
                    .Where(s => s.DoctorId == doctor.Id && !s.IsDeleted)
                    .ToListAsync(ct)
                : new List<LectureGroup>();

            var staffSection = new StaffProfileSection
            {
                Title = doctor != null
                    ? (doctor.IsAcademicAdvisor ? "أستاذ / مرشد أكاديمي" : "أستاذ دكتور")
                    : "موظف إداري",
                EmployeeCode = profile.Staff?.EmployeeCode ?? doctor?.EmployeeCode
            };

            foreach (var section in sections)
            {
                if (!staffSection.OfficeHours.Any(w =>
                        w.Day == section.Day &&
                        w.StartTime == section.StartTime?.ToString(@"hh\:mm") &&
                        w.EndTime == section.EndTime?.ToString(@"hh\:mm")))
                {
                    staffSection.OfficeHours.Add(new OfficeHourDto
                    {
                        Day = section.Day,
                        StartTime = section.StartTime?.ToString(@"hh\:mm"),
                        EndTime = section.EndTime?.ToString(@"hh\:mm")
                    });
                }
            }

            var uniqueCourses = sections
                .GroupBy(s => s.CourseId)
                .Select(g => g.First())
                .ToList();

            foreach (var section in uniqueCourses)
            {
                var term = section.CalendarEvents?
                    .Select(ce => ce.AcademicTerm)
                    .FirstOrDefault(t => t != null);

                var semesterName = "-";
                if (term != null)
                {
                    semesterName = $"{term.Name} {term.AcademicYear?.Name}";
                }

                staffSection.TaughtCourses.Add(new StaffCourseDto
                {
                    CourseCode = section.Course.Code,
                    CourseTitle = section.Course.Title,
                    SemesterName = semesterName
                });
            }

            response.Staff = staffSection;
        }

        private static Task LoadAdminProfileAsync(
            GetProfilesForAdminsResponse response,
            ProfileData profile,
            IRafeekDbContext context,
            CancellationToken ct)
        {
            response.Admin = new AdminProfileSection
            {
                Role = "مدير النظام"
            };

            return Task.CompletedTask;
        }

        private static List<UserType> GetFlags(UserType types)
        {
            return Enum.GetValues<UserType>()
                .Where(t => t != UserType.None && types.HasFlag(t))
                .ToList();
        }
    }
}
