using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AdminHandlers.Commands.UpdateSecurityDashboard
{
    public class UpdateSecurityDashboardCommandHandler
        : IRequestHandler<UpdateSecurityDashboardCommand, bool>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUser;
        private readonly ISecuritySettingCache _cache;
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateSecurityDashboardCommandHandler(
            IRafeekDbContext context,
            ICurrentUserService currentUser,
            ISecuritySettingCache cache,
            UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _currentUser = currentUser;
            _cache = cache;
            _userManager = userManager;
        }

        public async Task<bool> Handle(
            UpdateSecurityDashboardCommand request,
            CancellationToken ct)
        {
            var setting = await _context.SecuritySettings.FirstOrDefaultAsync(ct);
            if (setting == null)
            {
                setting = new SecuritySetting
                {
                    SessionTimeoutMinutes      = 15,
                    IsForcePasswordChangeEnabled = false
                };
                await _context.SecuritySettings.AddAsync(setting, ct);
            }

            if (request.SessionTimeoutMinutes.HasValue)
                setting.SessionTimeoutMinutes = request.SessionTimeoutMinutes.Value;

            if (request.IsForcePasswordChangeEnabled.HasValue)
            {
                setting.IsForcePasswordChangeEnabled =
                    request.IsForcePasswordChangeEnabled.Value;

                var allUsers = await _userManager.Users
                    .Where(u => u.Id != _currentUser.UserId)
                    .ToListAsync(ct);

                foreach (var user in allUsers)
                {
                    if (request.IsForcePasswordChangeEnabled.Value)
                    {
                        var lastChange = user.LastPasswordChangedAt ?? DateTime.MinValue;
                        var daysSinceChange = (DateTime.UtcNow - lastChange).TotalDays;
                        user.MustChangePassword = daysSinceChange >= 90;
                    }
                    else
                    {
                        user.MustChangePassword = false;
                    }
                }

                foreach (var user in allUsers)
                    await _userManager.UpdateAsync(user);
            }

            if (request.Permissions != null && request.Permissions.Any())
            {
                foreach (var item in request.Permissions)
                {
                    if (string.IsNullOrWhiteSpace(item.ModuleKey)) continue;

                    var perm = await _context.RolePermissions
                        .FirstOrDefaultAsync(rp => rp.ModuleKey == item.ModuleKey, ct);

                    if (perm == null)
                    {
                        perm = new RolePermission
                        {
                            ModuleKey    = item.ModuleKey,
                            ModuleNameAr = GetModuleNameAr(item.ModuleKey)
                        };
                        await _context.RolePermissions.AddAsync(perm, ct);
                    }

                    if (item.CanView.HasValue)   perm.CanView   = item.CanView.Value;
                    if (item.CanAdd.HasValue)    perm.CanAdd    = item.CanAdd.Value;
                    if (item.CanEdit.HasValue)   perm.CanEdit   = item.CanEdit.Value;
                    if (item.CanDelete.HasValue) perm.CanDelete = item.CanDelete.Value;
                    if (item.CanCreate.HasValue) perm.CanCreate = item.CanCreate.Value;
                }
            }

            await _context.SaveChangesAsync(ct);


            _cache.Update(
                setting.SessionTimeoutMinutes,
                setting.IsForcePasswordChangeEnabled);

            return true;
        }

        private static string GetModuleNameAr(string key) => key switch
        {
            "UserManagement"    => "إدارة المستخدمين",
            "FinanceAndReports" => "المالية والتقارير",
            "ContentManagement" => "إدارة المحتوى",
            "GeneralSettings"   => "الإعدادات العامة",
            _                   => key
        };
    }
}
