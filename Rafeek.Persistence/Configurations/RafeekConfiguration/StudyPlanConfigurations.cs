using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class StudyPlanConfigurations : IEntityTypeConfiguration<StudyPlan>
    {
        public void Configure(EntityTypeBuilder<StudyPlan> builder)
        {
            builder.HasOne(sp => sp.Student)
                .WithMany(s => s.StudyPlans)
                .HasForeignKey(sp => sp.StudentId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(sp => sp.Course)
                .WithMany(c => c.StudyPlans)
                .HasForeignKey(sp => sp.CourseId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasQueryFilter(sp => !sp.IsDeleted);
        }
    }
}
