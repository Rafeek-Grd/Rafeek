using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class StudentSupportConfigurations : IEntityTypeConfiguration<StudentSupport>
    {
        public void Configure(EntityTypeBuilder<StudentSupport> builder)
        {
            builder.HasOne(s => s.Student)
                   .WithMany(s => s.StudentSupports)
                   .HasForeignKey(s => s.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
