using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Persistence.Configurations.Identity
{
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.ToTable("Users");

            builder.HasQueryFilter(u => !u.IsDeleted);

            builder.HasOne(u => u.Department)
                .WithMany(d => d.Users)
                .HasForeignKey(u => u.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(u => u.Supervisor)
                .WithMany()
                .HasForeignKey(u => u.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Property(s => s.Language).HasDefaultValue(ApplicationLanguage.AR);
        }
    }
}
