using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdminHandlers.Queries;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard
{
    public class GetStaffDashboardQueryHandler : IRequestHandler<GetStaffDashboardQuery, GetStaffDashboardDto>
    {
        private readonly IRafeekDbContext _context;

        public GetStaffDashboardQueryHandler(IRafeekDbContext context)
        {
            _context = context;
        }

        public async Task<GetStaffDashboardDto> Handle(GetStaffDashboardQuery request, CancellationToken cancellationToken)
        {
            // ── 1. Statistics ────────────────────────────
            var allStudents = await _context.Students
                .AsNoTracking()
                .Select(s => s.Level)
                .ToListAsync(cancellationToken);

            int totalStudents = allStudents.Count == 0 ? 1 : allStudents.Count;
            int firstYearCount = allStudents.Count(l => l == 1);
            int secondYearCount = allStudents.Count(l => l == 2);
            int advancedCount = allStudents.Count(l => l >= 3);

            var profiles = await _context.StudentAcademicProfiles
                .AsNoTracking()
                .Select(p => p.CGPA)
                .ToListAsync(cancellationToken);

            int totalProfiles = profiles.Count == 0 ? 1 : profiles.Count;
            int stableCount = profiles.Count(gpa => gpa >= 2.0f);
            int warningCount = profiles.Count(gpa => gpa >= 1.0f && gpa < 2.0f);
            int monitoredCount = profiles.Count(gpa => gpa < 1.0f);

            int registrationHolds = await _context.StudentAcademicProfiles
                .AsNoTracking()
                .CountAsync(p => p.RemainingCredits > 90, cancellationToken);

            int academicProbation = profiles.Count(gpa => gpa < 1.5f);

            int missingRequirements = await _context.StudyPlans
                .AsNoTracking()
                .CountAsync(sp => !_context.Enrollments
                    .Any(e => e.StudentId == sp.StudentId && e.Grade != null), cancellationToken);

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
                    Label = new DateTime(grp.Key.Year, grp.Key.Month, 1)
                        .ToString("MMMM", new System.Globalization.CultureInfo("ar-SA")),
                    AverageGpa = MathF.Round(grp.Average(g => g.TermGPA), 2)
                })
                .ToList();

            // ── 2. Student Academic Records ────────────────────────────
            var recordsQuery = _context.Students
                .AsNoTracking()
                .Include(s => s.User)
                .Include(s => s.Department)
                .Include(s => s.AcademicProfile)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                var term = request.SearchTerm.Trim();
                recordsQuery = recordsQuery.Where(s =>
                    s.User.FullName.Contains(term) ||
                    s.UniversityCode.Contains(term) ||
                    (s.User.Email != null && s.User.Email.Contains(term)));
            }

            if (!string.IsNullOrWhiteSpace(request.AcademicStatus))
            {
                recordsQuery = recordsQuery.Where(s => s.AcademicProfile != null && s.AcademicProfile.Standing == request.AcademicStatus);
            }

            if (request.Cgpa.HasValue)
            {
                recordsQuery = recordsQuery.Where(s => s.AcademicProfile != null && s.AcademicProfile.CGPA == request.Cgpa.Value);
            }

            if (request.DepartmentId.HasValue)
            {
                recordsQuery = recordsQuery.Where(s => s.DepartmentId == request.DepartmentId.Value);
            }

            var totalRecordsCount = await recordsQuery.CountAsync(cancellationToken);

            var recordItems = await recordsQuery
                .OrderBy(s => s.User.FullName)
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(s => new StudentAcademicRecordDto
                {
                    StudentId = s.Id,
                    FullName = s.User.FullName,
                    UniversityEmail = s.User.Email!,
                    UniversityCode = s.UniversityCode,
                    DepartmentName = s.Department != null ? s.Department.Name : null,
                    Cgpa = s.AcademicProfile != null ? s.AcademicProfile.CGPA : 0f,
                    AcademicStatus = s.AcademicProfile != null ? s.AcademicProfile.Standing : "Stable",
                    AcademicStatusLabel = s.AcademicProfile == null ? "منتظم"
                                        : s.AcademicProfile.Standing == "Stable" ? "منتظم"
                                        : s.AcademicProfile.Standing == "Warning" ? "تحذير"
                                        : s.AcademicProfile.Standing == "Probation" ? "إنذار أول"
                                        : s.AcademicProfile.Standing,
                    Level = s.Level,
                    Term = s.Term
                })
                .ToListAsync(cancellationToken);

            var studentAcademicRecords = new PagginatedResult<StudentAcademicRecordDto>(
                recordItems.AsReadOnly(),
                totalRecordsCount,
                request.PageNumber,
                request.PageSize);

            // ── 3. Assemble ────────────────────────────
            return new GetStaffDashboardDto
            {
                AcademicLevelTrend = new AcademicLevelTrendDto
                {
                    ChangePercentage = changePercentage,
                    DataPoints = dataPoints
                },
                BatchDistribution = new BatchDistributionDto
                {
                    FirstYear = new BatchSliceDto { Count = firstYearCount, Percentage = MathF.Round(firstYearCount * 100f / totalStudents, 1) },
                    SecondYear = new BatchSliceDto { Count = secondYearCount, Percentage = MathF.Round(secondYearCount * 100f / totalStudents, 1) },
                    AdvancedYears = new BatchSliceDto { Count = advancedCount, Percentage = MathF.Round(advancedCount * 100f / totalStudents, 1) },
                },
                AcademicStatusAnalysis = new AcademicStatusAnalysisDto
                {
                    Stable = new StatusSliceDto { Count = stableCount, Percentage = MathF.Round(stableCount * 100f / totalProfiles, 1) },
                    Warning = new StatusSliceDto { Count = warningCount, Percentage = MathF.Round(warningCount * 100f / totalProfiles, 1) },
                    Monitored = new StatusSliceDto { Count = monitoredCount, Percentage = MathF.Round(monitoredCount * 100f / totalProfiles, 1) },
                },
                AcademicObstacles = new AcademicObstaclesDto
                {
                    RegistrationHolds = registrationHolds,
                    AcademicProbation = academicProbation,
                    MissingRequirements = missingRequirements
                },
                StudentAcademicRecords = studentAcademicRecords
            };
        }
    }
}
