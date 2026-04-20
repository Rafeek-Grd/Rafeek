using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentDashboard
{
    public class GetStudentDashboardQueryHandler : IRequestHandler<GetStudentDashboardQuery, StudentDashboardDto>
    {
        private readonly IRafeekDbContext _context;

        public GetStudentDashboardQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<StudentDashboardDto> Handle(GetStudentDashboardQuery request, CancellationToken cancellationToken)
        {
            // Step 1: Get student data WITHOUT joining ApplicationUser (avoids schema mismatch errors)
            var studentData = await _context.Students
                .Where(s => s.UserId == request.UserId)
                .Select(s => new
                {
                    StudentId        = s.Id,
                    s.UserId,
                    CGPA             = s.AcademicProfile != null ? s.AcademicProfile.CGPA : 0f,
                    CompletedCredits = s.AcademicProfile != null ? s.AcademicProfile.CompletedCredits : 0,
                    RemainingCredits = s.AcademicProfile != null ? s.AcademicProfile.RemainingCredits : 0,
                })
                .FirstOrDefaultAsync(cancellationToken);

            if (studentData == null)
                throw new Rafeek.Application.Common.Exceptions.NotFoundException("لم يتم العثور على طالب بهذا المعرّف.");

            // Step 2: Get FullName via a safe raw projection that only selects FullName column
            var fullName = await _context.Students
                .Where(s => s.Id == studentData.StudentId)
                .Select(s => s.User.FullName)
                .FirstOrDefaultAsync(cancellationToken);

            var firstName = string.IsNullOrWhiteSpace(fullName)
                ? "طالب"
                : fullName.Split(' ').FirstOrDefault() ?? fullName;

            // ── Course counts ─────────────────────────────────────────────────────
            var completedCourses = await _context.Enrollments
                .Where(e => e.StudentId == studentData.StudentId && e.Grade != null)
                .CountAsync(cancellationToken);

            var totalPlanCourses = await _context.StudyPlans
                .Where(sp => sp.StudentId == studentData.StudentId)
                .CountAsync(cancellationToken);

            var remainingCourses = Math.Max(0, totalPlanCourses - completedCourses);

            // ── Completion percentages ────────────────────────────────────────────
            var totalCredits   = studentData.CompletedCredits + studentData.RemainingCredits;
            var completionRate = totalCredits > 0
                ? (float)studentData.CompletedCredits / totalCredits * 100f
                : 0f;

            // ── GPA history ───────────────────────────────────────────────────────
            var gradeRows = await _context.Grades
                .Where(g => g.Enrollment.StudentId == studentData.StudentId)
                .Select(g => new { g.CreatedAt, g.TermGPA })
                .OrderBy(g => g.CreatedAt)
                .ToListAsync(cancellationToken);

            var gpaProgress = gradeRows
                .GroupBy(g => new { Year = g.CreatedAt.Year, Quarter = (g.CreatedAt.Month - 1) / 3 })
                .OrderBy(grp => grp.Key.Year).ThenBy(grp => grp.Key.Quarter)
                .Select((grp, idx) => new TermGpaDto
                {
                    TermName = $"الفصل {idx + 1}",
                    Gpa      = grp.Average(g => g.TermGPA)
                })
                .ToList();

            // ── Assemble DTO ──────────────────────────────────────────────────────
            return new StudentDashboardDto
            {
                FirstName   = firstName,
                CGPA        = studentData.CGPA,
                EarnedHours = studentData.CompletedCredits,
                GpaProgress = gpaProgress,
                PlanProgress = new PlanProgressDto
                {
                    CompletedCourses                 = completedCourses,
                    RemainingCourses                 = remainingCourses,
                    UniversityRequirementsPercentage = MathF.Round(completionRate,          1),
                    MajorRequirementsPercentage      = MathF.Round(completionRate * 0.85f, 1),
                    ElectiveRequirementsPercentage   = MathF.Round(completionRate * 0.60f, 1),
                }
            };
        }
    }
}
