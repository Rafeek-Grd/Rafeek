using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class LearningResourceConfigurations : IEntityTypeConfiguration<LearningResource>
    {
        public void Configure(EntityTypeBuilder<LearningResource> builder)
        {
            builder.HasOne(lr => lr.Course)
                    .WithMany(c => c.LearningResources)
                    .HasForeignKey(lr => lr.CourseId)
                    .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(lr => !lr.IsDeleted);
        }
    }
}
