using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class GPASimulatorLogConfiguration : IEntityTypeConfiguration<GPASimulatorLog>
    {
        public void Configure(EntityTypeBuilder<GPASimulatorLog> builder)
        {
            builder.ToTable(nameof(GPASimulatorLog));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.ExpectedGPA)
                .IsRequired();

            builder.Property(x => x.PredictedCGPA)
                .IsRequired();

            // Relations
            builder.HasOne(x => x.Student)
                .WithMany(x => x.GPASimulatorLogs)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.StudentId);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
