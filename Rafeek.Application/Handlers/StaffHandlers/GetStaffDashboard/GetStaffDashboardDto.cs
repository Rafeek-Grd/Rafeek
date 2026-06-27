using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdminHandlers.Queries;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard;

namespace Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard
{
    public class GetStaffDashboardDto
    {
        public AcademicLevelTrendDto AcademicLevelTrend { get; set; } = new();
        public BatchDistributionDto BatchDistribution { get; set; } = new();
        public AcademicStatusAnalysisDto AcademicStatusAnalysis { get; set; } = new();
        public AcademicObstaclesDto AcademicObstacles { get; set; } = new();
        public PagginatedResult<StudentAcademicRecordDto> StudentAcademicRecords { get; set; } = null!;
    }
}
