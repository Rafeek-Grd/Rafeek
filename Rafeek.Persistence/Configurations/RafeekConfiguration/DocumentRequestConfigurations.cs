using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class DocumentRequestConfigurations : IEntityTypeConfiguration<DocumentRequest>
    {
        public void Configure(EntityTypeBuilder<DocumentRequest> builder)
        {
            builder.HasOne(dr => dr.Student)
                   .WithMany(s => s.DocumentRequests)
                   .HasForeignKey(dr => dr.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(dr => !dr.IsDeleted);
        }
    }
}
