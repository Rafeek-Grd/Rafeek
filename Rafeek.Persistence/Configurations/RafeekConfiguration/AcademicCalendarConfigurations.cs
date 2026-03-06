using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AcademicCalendarConfigurations : IEntityTypeConfiguration<AcademicCalendar>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<AcademicCalendar> builder)
        {
            builder.HasOne(ac => ac.TargetUser)
                   .WithMany(u => u.GuidanceEvents)
                   .HasForeignKey(ac => ac.TargetUserId)
                   .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
