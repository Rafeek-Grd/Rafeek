using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class CourseSectionConfiguration : IEntityTypeConfiguration<CourseSection>
    {
        public void Configure(EntityTypeBuilder<CourseSection> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Day)
                .IsRequired();

            builder.Property(x => x.StartTime)
                .IsRequired();

            builder.Property(x => x.Duration)
                .IsRequired();

            builder.Property(x => x.Capacity)
                .IsRequired();

            builder.Property(x => x.AvailableSeats)
                .IsRequired();

            builder.HasOne(x => x.Course)
                .WithMany()
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
