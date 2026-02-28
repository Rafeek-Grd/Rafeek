using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class DoctorConfigurations : IEntityTypeConfiguration<Doctor>
    {
        public void Configure(EntityTypeBuilder<Doctor> builder)
        {
            builder.HasOne(d => d.User)
                   .WithOne()
                   .HasForeignKey<Doctor>(d => d.UserId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(d => d.AdvisedStudents)
                   .WithOne(s => s.AcademicAdvisor)
                   .HasForeignKey(s => s.AcademicAdvisorId)
                   .OnDelete(DeleteBehavior.NoAction);

            builder.HasQueryFilter(d => !d.IsDeleted);
        }
    }
}
