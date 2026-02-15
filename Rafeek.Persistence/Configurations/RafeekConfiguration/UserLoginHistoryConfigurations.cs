using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class UserLoginHistoryConfigurations : IEntityTypeConfiguration<UserLoginHistory>
    {
        public void Configure(EntityTypeBuilder<UserLoginHistory> builder)
        {
            builder.HasOne(h => h.ApplicationUser)
                   .WithMany(u => u.LoginHistories)
                   .HasForeignKey(h => h.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(h => !h.IsDeleted);
        }
    }
}
