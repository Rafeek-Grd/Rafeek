using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AcademicTermConfigurations : IEntityTypeConfiguration<AcademicTerm>
    {
        public void Configure(EntityTypeBuilder<AcademicTerm> builder)
        {
            builder.ToTable("AcademicTerms");

            builder.HasOne(x => x.AcademicYear)
                .WithMany(y => y.Terms)
                .HasForeignKey(x => x.AcademicYearId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
