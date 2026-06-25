using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class GradeScaleConfiguration : IEntityTypeConfiguration<GradeScale>
    {
        public void Configure(EntityTypeBuilder<GradeScale> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.GradeLetter).IsRequired().HasMaxLength(10);
            builder.Property(x => x.ArabicDescription).IsRequired().HasMaxLength(50);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
