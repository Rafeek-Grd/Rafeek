using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AcadmicFeedbackConfigurations : IEntityTypeConfiguration<AcademicFeedback>
    {
        public void Configure(EntityTypeBuilder<AcademicFeedback> builder)
        {
            builder.HasOne(af => af.Student)
                   .WithMany(s => s.AcademicFeedbacks)
                   .HasForeignKey(af => af.StudentId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(af => !af.IsDeleted);
        }
    }
}
