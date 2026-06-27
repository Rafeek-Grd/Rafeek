using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class CourseRegistrationPeriodConfiguration : IEntityTypeConfiguration<CourseRegistrationPeriod>
    {
        public void Configure(EntityTypeBuilder<CourseRegistrationPeriod> builder)
        {
            builder.HasOne(crp => crp.Course)
                   .WithMany(c => c.RegistrationPeriods)
                   .HasForeignKey(crp => crp.CourseId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(crp => crp.AcademicTerm)
                   .WithMany()
                   .HasForeignKey(crp => crp.AcademicTermId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(crp => !crp.IsDeleted);
        }
    }
}
