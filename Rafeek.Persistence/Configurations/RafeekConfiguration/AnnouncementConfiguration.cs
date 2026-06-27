using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AnnouncementConfiguration : IEntityTypeConfiguration<Announcement>
    {
        public void Configure(EntityTypeBuilder<Announcement> builder)
        {
            builder.HasOne(a => a.Department)
                   .WithMany()
                   .HasForeignKey(a => a.DepartmentId)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(a => a.ScheduledAt);

            builder.HasQueryFilter(a => !a.IsDeleted);
        }
    }
}
