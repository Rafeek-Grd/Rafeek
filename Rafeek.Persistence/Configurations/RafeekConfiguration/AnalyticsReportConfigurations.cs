using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AnalyticsReportConfigurations : IEntityTypeConfiguration<AnalyticsReport>
    {
        public void Configure(EntityTypeBuilder<AnalyticsReport> builder)
        {
            builder.ToTable("AnalyticsReports", "dbo");

            builder.HasKey(ar => ar.Id);

            builder.Property(ar => ar.ReportType)
                   .IsRequired();

            builder.Property(ar => ar.GeneratedAt)
                   .IsRequired();

            builder.Property(ar => ar.FileUrl)
                   .IsRequired()
                   .HasMaxLength(500);

            builder.HasOne(ar => ar.Student)
                   .WithMany(s => s.AnalyticsReports)
                   .HasForeignKey(ar => ar.StudentId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(ar => !ar.IsDeleted);

            builder.HasIndex(ar => ar.StudentId);
            builder.HasIndex(ar => ar.ReportType);
            builder.HasIndex(ar => ar.GeneratedAt);
        }
    }
}
