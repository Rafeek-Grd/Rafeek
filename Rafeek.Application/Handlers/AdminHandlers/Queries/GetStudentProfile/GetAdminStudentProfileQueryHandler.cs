using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentProfile
{
    public class GetAdminStudentProfileQueryHandler : IRequestHandler<GetAdminStudentProfileQuery, AdminStudentProfileDto>
    {
        private readonly IRafeekDbContext _context;

        public GetAdminStudentProfileQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<AdminStudentProfileDto> Handle(GetAdminStudentProfileQuery request, CancellationToken cancellationToken)
        {
            var studentId = request.StudentId;
            var student = await _context.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.AcademicAdvisor)
                    .ThenInclude(d => d!.User)
                .Include(s => s.AcademicProfile)
                .FirstOrDefaultAsync(s => s.Id == studentId || s.UserId == studentId, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException($"الطالب بالمعرّف {studentId} غير موجود.");
            }

            var enrollments = await _context.Enrollments
                .AsNoTracking()
                .Include(e => e.Course)
                .Include(e => e.Grades)
                .Where(e => e.StudentId == student.Id && !e.IsDeleted)
                .ToListAsync(cancellationToken);

            var sectionIds = enrollments.Select(e => e.LectureGroupId).Distinct().ToList();
            var sections = await _context.LectureGroups
                .IgnoreQueryFilters()
                .AsNoTracking()
                .Include(s => s.Doctor)
                    .ThenInclude(d => d!.User)
                .Include(s => s.CalendarEvents)
                    .ThenInclude(ce => ce.AcademicTerm)
                        .ThenInclude(at => at!.AcademicYear)
                .Where(s => sectionIds.Contains(s.Id))
                .ToListAsync(cancellationToken);
            var sectionsDict = sections.ToDictionary(s => s.Id);

            var allGrades = enrollments.SelectMany(e => e.Grades).ToList();
            float currentGpa = 0, cumulativeGpa = 0;
            if (allGrades.Any())
            {
                var latestGrade = allGrades.OrderByDescending(g => g.CreatedAt).First();
                currentGpa = latestGrade.TermGPA;
                cumulativeGpa = latestGrade.CGPA;
            }

            int level = (student.AcademicProfile?.CompletedCredits ?? 0) / 30 + 1;
            string levelName = level switch
            {
                1 => "الفرقة الأولى",
                2 => "الفرقة الثانية",
                3 => "الفرقة الثالثة",
                4 => "الفرقة الرابعة",
                _ => $"المستوى {level}"
            };

            var dto = new AdminStudentProfileDto
            {
                StudentId = student.Id,
                FullName = student.User.FullName,
                Email = student.User.Email,
                UniversityCode = student.UniversityCode,
                DepartmentName = student.Department?.Name ?? "-",
                Level = level,
                LevelName = levelName,
                AcademicAdvisorName = student.AcademicAdvisor?.User?.FullName ?? "-",
                ProfilePictureUrl = student.User.ProfilePictureUrl,
                CurrentGPA = currentGpa,
                CumulativeGPA = cumulativeGpa
            };

            foreach (var enrollment in enrollments)
            {
                var lectureGroup = sectionsDict.GetValueOrDefault(enrollment.LectureGroupId);

                bool isCompleted = !string.IsNullOrEmpty(enrollment.Grade);
                
                if (!isCompleted)
                {
                    dto.CurrentEnrollments.Add(new AdminStudentCurrentEnrollmentDto
                    {
                        CourseCode = enrollment.Course.Code,
                        CourseTitle = enrollment.Course.Title,
                        InstructorName = lectureGroup?.Doctor?.User?.FullName ?? "-",
                        Status = "Enrolled",
                        StatusLabel = "مسجل"
                    });
                }
                else
                {
                    var finalGradeDetails = enrollment.Grades.OrderByDescending(g => g.AbsoluteScore).FirstOrDefault();
                    
                    var term = lectureGroup?.CalendarEvents?
                        .Select(ce => ce.AcademicTerm)
                        .FirstOrDefault(at => at != null);
                    
                    string semesterName = "-";
                    if (term != null)
                    {
                        semesterName = $"{term.Name} {term.AcademicYear?.Name}";
                    }

                    dto.ResultsHistory.Add(new AdminStudentResultHistoryDto
                    {
                        CourseCode = enrollment.Course.Code,
                        CourseTitle = enrollment.Course.Title,
                        SemesterName = semesterName,
                        Score = finalGradeDetails?.AbsoluteScore,
                        Grade = enrollment.Grade ?? "N/A"
                    });
                }
            }

            dto.ResultsHistory = dto.ResultsHistory
                .OrderByDescending(r => r.SemesterName)
                .ThenBy(r => r.CourseCode)
                .ToList();

            return dto;
        }
    }
}
