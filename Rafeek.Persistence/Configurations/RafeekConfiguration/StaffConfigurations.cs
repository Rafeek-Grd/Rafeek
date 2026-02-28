using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class StaffConfigurations : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.HasOne(s => s.User)
                   .WithOne()
                   .HasForeignKey<Staff>(s => s.UserId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
