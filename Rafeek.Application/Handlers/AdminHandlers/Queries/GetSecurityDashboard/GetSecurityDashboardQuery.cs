using MediatR;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetSecurityDashboard
{
    public class GetSecurityDashboardQuery : IRequest<SecurityDashboardDto> { }

    public class SecurityDashboardDto
    {
        public int NewUsersCount { get; set; }
        public int ActiveUsersCount { get; set; }
        public int TotalUsersCount { get; set; }

        public List<ChartItemDto> SystemUsageRate { get; set; } = new();

        public List<ChartItemDto> RolesDistribution { get; set; } = new();

        public List<int> LoginChartThisMonth { get; set; } = new();
        public List<int> LoginChartLastMonth { get; set; } = new();

        public SecurityPoliciesDto Policies { get; set; } = new();

        public List<SuspiciousIPDto> SuspiciousIPs { get; set; } = new();

        public List<LiveActivityDto> LiveActivities { get; set; } = new();

        public List<PermissionMatrixItemDto> PermissionsMatrix { get; set; } = new();
    }

    public class ChartItemDto
    {
        public string Label { get; set; } = null!;
        public int Percentage { get; set; }
        public string? ColorHex { get; set; }
    }

    public class SecurityPoliciesDto
    {
        public bool TwoFactorAuthEnabled { get; set; }
        public bool PasswordChangeForced { get; set; }
        public string SessionTimeout { get; set; } = null!;
    }

    public class SuspiciousIPDto
    {
        public string IP { get; set; } = null!;
        public string LocationInfo { get; set; } = null!;
    }

    public class LiveActivityDto
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string TimeLabel { get; set; } = null!;
        public string TypeIcon { get; set; } = null!;
    }

    public class PermissionMatrixItemDto
    {
        public string ModuleName { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool CanView { get; set; }
        public bool CanEdit { get; set; }
        public bool CanCreate { get; set; }
        public bool CanDelete { get; set; }
        public bool CanExport { get; set; }
    }
}
