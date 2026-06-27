using Rafeek.Application.Handlers.AdminHandlers.Queries;
using Rafeek.Application.Handlers.AdminHandlers.Queries.GetAdminDashboard;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.StaffHandlers.GetStaffDashboard
{
    public class GetStaffDashboardDto
    {
        public AcademicLevelTrendDto AcademicLevelTrend { get; set; } = new();
        public BatchDistributionDto BatchDistribution { get; set; } = new();
        public AcademicStatusAnalysisDto AcademicStatusAnalysis { get; set; } = new();
        public AcademicObstaclesDto AcademicObstacles { get; set; } = new();
        public List<StudentAcademicRecordDto> StudentAcademicRecords { get; set; } = new();
    }
}
