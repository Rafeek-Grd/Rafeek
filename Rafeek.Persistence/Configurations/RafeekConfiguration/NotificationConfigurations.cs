using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class NotificationConfigurations : IEntityTypeConfiguration<Notification>
    {
        public void Configure(EntityTypeBuilder<Notification> builder)
        {
            // العلاقة مع Student (اختيارية)
            builder.HasOne(n => n.Student)
                   .WithMany(s => s.Notifications)
                   .HasForeignKey(n => n.StudentId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .IsRequired(false);

            // Type
            builder.Property(n => n.Type)
                   .IsRequired()
                   .HasConversion<int>()
                   .HasDefaultValue(NotificationType.Academic);

            // Title
            builder.Property(n => n.Title)
                   .IsRequired()
                   .HasMaxLength(255);

            // Message
            builder.Property(n => n.Message)
                   .IsRequired();

            // TargetGroup (اختياري)
            builder.Property(n => n.TargetGroup)
                   .HasMaxLength(100);

            // SendTime (اختياري)
            builder.Property(n => n.SendTime)
                   .IsRequired(false);

            // IsRead
            builder.Property(n => n.IsRead)
                   .IsRequired()
                   .HasDefaultValue(false);

            // Query Filter
            builder.HasQueryFilter(n => !n.IsDeleted);

            // Indexes
            builder.HasIndex(n => n.StudentId);
            builder.HasIndex(n => n.Type);
            builder.HasIndex(n => n.SendTime);
        }
    }
}
