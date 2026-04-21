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
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Course)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Section)
                        .ThenInclude(sec => sec.Instructor)
                            .ThenInclude(i => i.User)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Section)
                        .ThenInclude(sec => sec.CalendarEvents)
                            .ThenInclude(ce => ce.AcademicTerm)
                                .ThenInclude(at => at!.AcademicYear)
                .Include(s => s.Enrollments)
                    .ThenInclude(e => e.Grades)
                .FirstOrDefaultAsync(s => s.Id == studentId || s.UserId == studentId, cancellationToken);

            if (student == null)
            {
                throw new NotFoundException($"الطالب بالمعرّف {studentId} غير موجود.");
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
                ProfilePictureUrl = student.User.ProfilePictureUrl
            };

            foreach (var enrollment in student.Enrollments)
            {
                bool isCompleted = !string.IsNullOrEmpty(enrollment.Grade);
                
                if (!isCompleted)
                {
                    dto.CurrentEnrollments.Add(new AdminStudentCurrentEnrollmentDto
                    {
                        CourseCode = enrollment.Course.Code,
                        CourseTitle = enrollment.Course.Title,
                        InstructorName = enrollment.Section?.Instructor?.User?.FullName ?? "-",
                        Status = "Enrolled",
                        StatusLabel = "مسجل"
                    });
                }
                else
                {
                    var finalGradeDetails = enrollment.Grades.OrderByDescending(g => g.AbsoluteScore).FirstOrDefault();
                    
                    var term = enrollment.Section?.CalendarEvents?
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
