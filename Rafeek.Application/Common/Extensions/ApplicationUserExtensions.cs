using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Common.Extensions
{
    public static class ApplicationUserExtensions
    {
        public static async Task<string> GetUserRoleStringAsync(this ApplicationUser user, IRafeekDbContext dbContext, CancellationToken cancellationToken)
        {
            if (user.UserTypes.HasFlag(UserType.Admin) || user.UserTypes.HasFlag(UserType.SubAdmin))
            {
                return "admin";
            }

            if (user.UserTypes.HasFlag(UserType.Staff))
            {
                return "staff";
            }

            if (user.UserTypes.HasFlag(UserType.Doctor))
            {
                var localDoctor = dbContext.Doctors.Local.FirstOrDefault(d => d.UserId == user.Id);
                if (localDoctor != null)
                {
                    return localDoctor.IsAcademicAdvisor ? "mentor" : "professor";
                }

                var isAdvisor = await dbContext.Doctors.AsNoTracking()
                    .Where(d => d.UserId == user.Id)
                    .Select(d => d.IsAcademicAdvisor)
                    .FirstOrDefaultAsync(cancellationToken);

                return isAdvisor ? "mentor" : "professor";
            }

            if (user.UserTypes.HasFlag(UserType.Instructor))
            {
                return "professor";
            }

            if (user.UserTypes.HasFlag(UserType.Student))
            {
                return "student";
            }

            return "student";
        }
    }
}
