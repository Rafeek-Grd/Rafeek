using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class CoursePrerequisiteConfiguration : IEntityTypeConfiguration<CoursePrerequisite>
    {
        public void Configure(EntityTypeBuilder<CoursePrerequisite> builder)
        {
            builder.ToTable("CoursePrerequisites");

            builder.HasKey(x => new { x.CourseId, x.PrerequisiteId});

            builder.Ignore(x => x.Id);

            builder.HasOne(x => x.Course)
                .WithMany(x => x.Prerequisites)
                .HasForeignKey(x => x.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Prerequisite)
                .WithMany()
                .HasForeignKey(x => x.PrerequisiteId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}