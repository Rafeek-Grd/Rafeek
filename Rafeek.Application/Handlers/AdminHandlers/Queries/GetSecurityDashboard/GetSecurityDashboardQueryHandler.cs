using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetSecurityDashboard
{
    public class GetSecurityDashboardQueryHandler : IRequestHandler<GetSecurityDashboardQuery, SecurityDashboardDto>
    {
        private readonly IRafeekDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetSecurityDashboardQueryHandler(IRafeekDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<SecurityDashboardDto> Handle(GetSecurityDashboardQuery request, CancellationToken cancellationToken)
        {
            var dto = new SecurityDashboardDto();

            var allUsers = await _userManager.Users.AsNoTracking().ToListAsync(cancellationToken);

            dto.TotalUsersCount = allUsers.Count;
            dto.ActiveUsersCount = allUsers.Count(u => u.IsUniversityEmailActivated);
            dto.NewUsersCount = allUsers.Count(u => !u.IsUniversityEmailActivated);

            var studentCount = await _context.Students.AsNoTracking().CountAsync(cancellationToken);
            var instructorCount = await _context.Instructors.AsNoTracking().CountAsync(cancellationToken);
            var doctorCount = await _context.Doctors.AsNoTracking().CountAsync(cancellationToken);
            var staffCount = await _context.Staffs.AsNoTracking().CountAsync(cancellationToken);
            int total = studentCount + instructorCount + doctorCount + staffCount;

            dto.SystemUsageRate = new List<ChartItemDto>();
            if (total > 0)
            {
                dto.SystemUsageRate.Add(new ChartItemDto { Label = "طلاب", Percentage = (int)Math.Round((double)studentCount / total * 100), ColorHex = "#1D4ED8" });
                dto.SystemUsageRate.Add(new ChartItemDto { Label = "هيئة تدريس", Percentage = (int)Math.Round((double)(instructorCount + doctorCount) / total * 100), ColorHex = "#93C5FD" });
                dto.SystemUsageRate.Add(new ChartItemDto { Label = "إداريون", Percentage = (int)Math.Round((double)staffCount / total * 100), ColorHex = "#EFF6FF" });
            }

            dto.RolesDistribution = dto.SystemUsageRate;

            var now = DateTime.UtcNow;
            var loginHistories = await _context.UserLoginHistories
                .AsNoTracking()
                .Where(h => h.LoginTime >= now.AddDays(-60))
                .ToListAsync(cancellationToken);

            dto.LoginChartThisMonth = Enumerable.Range(0, 10)
                .Select(i => loginHistories.Count(h => h.LoginTime >= now.AddDays(-3 * (i + 1)) && h.LoginTime < now.AddDays(-3 * i)))
                .Reverse()
                .ToList();

            dto.LoginChartLastMonth = Enumerable.Range(0, 10)
                .Select(i => loginHistories.Count(h => h.LoginTime >= now.AddDays(-30 - 3 * (i + 1)) && h.LoginTime < now.AddDays(-30 - 3 * i)))
                .Reverse()
                .ToList();

            dto.Policies = new SecurityPoliciesDto
            {
                TwoFactorAuthEnabled = false,
                PasswordChangeForced = false,
                SessionTimeout = "30m"
            };

            dto.SuspiciousIPs = await _context.UserLoginHistories
                .AsNoTracking()
                .Where(h => h.IpAddress != null)
                .GroupBy(h => h.IpAddress!)
                .Where(g => g.Count() > 5)
                .Select(g => new SuspiciousIPDto
                {
                    IP = g.Key,
                    LocationInfo = "unknown"
                })
                .Take(10)
                .ToListAsync(cancellationToken);

            dto.LiveActivities = await _context.UserLoginHistories
                .AsNoTracking()
                .Include(h => h.ApplicationUser)
                .OrderByDescending(h => h.LoginTime)
                .Take(10)
                .Select(h => new LiveActivityDto
                {
                    Title = "تسجيل دخول",
                    Description = h.ApplicationUser.FullName + (h.IpAddress != null ? " - IP: " + h.IpAddress : ""),
                    TimeLabel = h.LoginTime.ToString("hh:mm tt"),
                    TypeIcon = "info"
                })
                .ToListAsync(cancellationToken);

            dto.PermissionsMatrix = new List<PermissionMatrixItemDto>
            {
                new PermissionMatrixItemDto { ModuleName = "إدارة المستخدمين", Description = "الوصول لحسابات الموظفين", CanView = true, CanEdit = true, CanCreate = true, CanDelete = true, CanExport = true },
                new PermissionMatrixItemDto { ModuleName = "المالية والفواتير", Description = "إدارة المدفوعات والاشتراكات", CanView = true, CanEdit = false, CanCreate = false, CanDelete = false, CanExport = false },
                new PermissionMatrixItemDto { ModuleName = "إدارة المحتوى", Description = "المقالات والوسائط", CanView = true, CanEdit = true, CanCreate = true, CanDelete = true, CanExport = false },
                new PermissionMatrixItemDto { ModuleName = "الإعدادات العامة", Description = "تكوين النظام وتفضيلات الموقع", CanView = true, CanEdit = true, CanCreate = true, CanDelete = true, CanExport = false }
            };

            return dto;
        }
    }
}
