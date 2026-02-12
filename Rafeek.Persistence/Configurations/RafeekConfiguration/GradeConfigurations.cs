using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class GradeConfigurations : IEntityTypeConfiguration<Grade>
    {
        public void Configure(EntityTypeBuilder<Grade> builder)
        {
            builder.HasOne(g => g.Enrollment)
                   .WithMany(e => e.Grades)
                   .HasForeignKey(g => g.EnrollmentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(g => !g.IsDeleted);
        }
    }
}
