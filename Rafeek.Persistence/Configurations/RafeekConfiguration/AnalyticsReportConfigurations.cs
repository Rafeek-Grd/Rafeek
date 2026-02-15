using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AnalyticsReportConfigurations : IEntityTypeConfiguration<AnalyticsReport>
    {
        public void Configure(EntityTypeBuilder<AnalyticsReport> builder)
        {
            builder.HasOne(ar => ar.Student)
                   .WithMany(s => s.AnalyticsReports)
                   .HasForeignKey(ar => ar.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(ar => !ar.IsDeleted);
        }
    }
}
