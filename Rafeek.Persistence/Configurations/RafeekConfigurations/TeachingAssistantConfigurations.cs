using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.Business
{
    public class TeachingAssistantConfigurations : IEntityTypeConfiguration<TeachingAssistant>
    {
        public void Configure(EntityTypeBuilder<TeachingAssistant> builder)
        {
            builder.HasOne(ta => ta.Department)
                .WithMany(d => d.TeachingAssistants)
                .HasForeignKey(ta => ta.DepartmentId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(ta => ta.Supervisor)
                .WithMany()
                .HasForeignKey(ta => ta.SupervisorId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
