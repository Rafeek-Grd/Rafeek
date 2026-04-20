using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AITimetableConfiguration : IEntityTypeConfiguration<AITimetable>
    {
        public void Configure(EntityTypeBuilder<AITimetable> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.OptionName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.TimetableName)
                .HasMaxLength(500);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(x => x.Items)
                .WithOne(x => x.Timetable)
                .HasForeignKey(x => x.TimetableId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }

    public class AITimetableItemConfiguration : IEntityTypeConfiguration<AITimetableItem>
    {
        public void Configure(EntityTypeBuilder<AITimetableItem> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.CourseName)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(x => x.Type)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.EndTime)
                .IsRequired();
        }
    }
}
