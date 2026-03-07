using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AcademicCalendarConfigurations : IEntityTypeConfiguration<AcademicCalendar>
    {
        public void Configure(EntityTypeBuilder<AcademicCalendar> builder)
        {
            builder.ToTable("AcademicCalendars");

            builder.HasOne(ac => ac.TargetUser)
                   .WithMany(u => u.GuidanceEvents)
                   .HasForeignKey(ac => ac.TargetUserId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ac => ac.AcademicTerm)
                   .WithMany(t => t.CalendarEvents)
                   .HasForeignKey(ac => ac.AcademicTermId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ac => ac.Department)
                   .WithMany(d => d.CalendarEvents)
                   .HasForeignKey(ac => ac.DepartmentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ac => ac.Course)
                   .WithMany(c => c.CalendarEvents)
                   .HasForeignKey(ac => ac.CourseId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.HasOne(ac => ac.Section)
                   .WithMany(s => s.CalendarEvents)
                   .HasForeignKey(ac => ac.SectionId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
