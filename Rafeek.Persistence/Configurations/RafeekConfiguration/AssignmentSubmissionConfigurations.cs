using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AssignmentSubmissionConfigurations : IEntityTypeConfiguration<AssignmentSubmission>
    {
        public void Configure(EntityTypeBuilder<AssignmentSubmission> builder)
        {
            builder.ToTable(nameof(AssignmentSubmission));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.SubmissionUrl)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(x => x.Feedback)
                .HasMaxLength(2000);

            builder.Property(x => x.Score);

            builder.Property(x => x.SubmittedAt)
                .IsRequired();

            builder.HasOne(x => x.Assignment)
                .WithMany(a => a.Submissions)
                .HasForeignKey(x => x.AssignmentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(x => x.Student)
                .WithMany()
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
