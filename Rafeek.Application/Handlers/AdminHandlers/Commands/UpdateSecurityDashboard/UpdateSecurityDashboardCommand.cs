using MediatR;

namespace Rafeek.Application.Handlers.AdminHandlers.Commands.UpdateSecurityDashboard
{
    public class UpdateSecurityDashboardCommand : IRequest<bool>
    {
        public int? SessionTimeoutMinutes { get; set; }
        public bool? IsForcePasswordChangeEnabled { get; set; }
        public List<RolePermissionUpdateItem>? Permissions { get; set; }
    }

    public class RolePermissionUpdateItem
    {
        public string? ModuleKey { get; set; }

        public bool? CanView   { get; set; }
        public bool? CanAdd    { get; set; }
        public bool? CanEdit   { get; set; }
        public bool? CanDelete { get; set; }
        public bool? CanCreate { get; set; }
    }
}
