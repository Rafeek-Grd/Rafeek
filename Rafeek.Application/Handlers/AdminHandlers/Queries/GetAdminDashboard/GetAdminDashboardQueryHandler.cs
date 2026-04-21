using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard
{
    public class GetAdminDashboardQueryHandler : IRequestHandler<GetAdminDashboardQuery, AdminDashboardDto>
    {
        private readonly IRafeekDbContext _context;

        public GetAdminDashboardQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<AdminDashboardDto> Handle(GetAdminDashboardQuery request, CancellationToken cancellationToken)
        {
            // ── 1. Batch Distribution (توزيع الدفعات) ────────────────────────────
            var allStudents = await _context.Students
                .AsNoTracking()
                .Select(s => s.Level)
                .ToListAsync(cancellationToken);

            int totalStudents = allStudents.Count == 0 ? 1 : allStudents.Count;
            int firstYearCount     = allStudents.Count(l => l == 1);
            int secondYearCount    = allStudents.Count(l => l == 2);
            int advancedCount      = allStudents.Count(l => l >= 3);

            // ── 2. Academic Status Analysis (تحليل الحالة الأكاديمية) ──────────
            var profiles = await _context.StudentAcademicProfiles
                .AsNoTracking()
                .Select(p => p.CGPA)
                .ToListAsync(cancellationToken);

            int totalProfiles  = profiles.Count == 0 ? 1 : profiles.Count;
            int stableCount    = profiles.Count(gpa => gpa >= 2.0f);
            int warningCount   = profiles.Count(gpa => gpa >= 1.0f && gpa < 2.0f);
            int monitoredCount = profiles.Count(gpa => gpa < 1.0f);

            // ── 3. Academic Obstacles (العوائق الأكاديمية) ────────────────────
            // Registration holds = students missing required credits to register
            int registrationHolds = await _context.StudentAcademicProfiles
                .AsNoTracking()
                .CountAsync(p => p.RemainingCredits > 90, cancellationToken);

            // Academic probation = CGPA < 1.5
            int academicProbation = profiles.Count(gpa => gpa < 1.5f);

            // Missing requirements = students with incomplete study plans vs enrollments
            int missingRequirements = await _context.StudyPlans
                .AsNoTracking()
                .CountAsync(sp => !_context.Enrollments
                    .Any(e => e.StudentId == sp.StudentId && e.Grade != null), cancellationToken);

            // ── 4. Academic Level Trend (معدل مستوى الطلاب) ──────────────────
            var gradeHistory = await _context.Grades
                .AsNoTracking()
                .Select(g => new { g.CreatedAt, g.TermGPA })
                .OrderBy(g => g.CreatedAt)
                .ToListAsync(cancellationToken);

            var previousPeriodAvg = gradeHistory
                .Where(g => g.CreatedAt < DateTime.UtcNow.AddMonths(-6))
                .Select(g => g.TermGPA)
                .DefaultIfEmpty(0)
                .Average();

            var currentPeriodAvg = gradeHistory
                .Where(g => g.CreatedAt >= DateTime.UtcNow.AddMonths(-6))
                .Select(g => g.TermGPA)
                .DefaultIfEmpty(0)
                .Average();

            var changePercentage = previousPeriodAvg > 0
                ? MathF.Round((currentPeriodAvg - previousPeriodAvg) / previousPeriodAvg * 100f, 1)
                : 0f;

            var dataPoints = gradeHistory
                .GroupBy(g => new { g.CreatedAt.Year, g.CreatedAt.Month })
                .OrderBy(grp => grp.Key.Year).ThenBy(grp => grp.Key.Month)
                .Select(grp => new GpaTrendPointDto
                {
                    Label      = new DateTime(grp.Key.Year, grp.Key.Month, 1)
                        .ToString("MMMM", new System.Globalization.CultureInfo("ar-SA")),
                    AverageGpa = MathF.Round(grp.Average(g => g.TermGPA), 2)
                })
                .ToList();

            // ── Assemble ─────────────────────────────────────────────────────────
            return new AdminDashboardDto
            {
                AcademicLevelTrend = new AcademicLevelTrendDto
                {
                    ChangePercentage = changePercentage,
                    DataPoints       = dataPoints
                },
                BatchDistribution = new BatchDistributionDto
                {
                    FirstYear    = new BatchSliceDto { Count = firstYearCount,  Percentage = MathF.Round(firstYearCount  * 100f / totalStudents, 1) },
                    SecondYear   = new BatchSliceDto { Count = secondYearCount, Percentage = MathF.Round(secondYearCount * 100f / totalStudents, 1) },
                    AdvancedYears= new BatchSliceDto { Count = advancedCount,   Percentage = MathF.Round(advancedCount   * 100f / totalStudents, 1) },
                },
                AcademicStatusAnalysis = new AcademicStatusAnalysisDto
                {
                    Stable    = new StatusSliceDto { Count = stableCount,    Percentage = MathF.Round(stableCount    * 100f / totalProfiles, 1) },
                    Warning   = new StatusSliceDto { Count = warningCount,   Percentage = MathF.Round(warningCount   * 100f / totalProfiles, 1) },
                    Monitored = new StatusSliceDto { Count = monitoredCount, Percentage = MathF.Round(monitoredCount * 100f / totalProfiles, 1) },
                },
                AcademicObstacles = new AcademicObstaclesDto
                {
                    RegistrationHolds   = registrationHolds,
                    AcademicProbation   = academicProbation,
                    MissingRequirements = missingRequirements
                }
            };
        }
    }
}
