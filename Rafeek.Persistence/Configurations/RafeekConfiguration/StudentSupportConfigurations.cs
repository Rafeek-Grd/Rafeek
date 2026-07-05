using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class StudentSupportConfigurations : IEntityTypeConfiguration<StudentSupport>
    {
        public void Configure(EntityTypeBuilder<StudentSupport> builder)
        {
            builder.HasOne(s => s.Student)
                   .WithMany(s => s.StudentSupports)
                   .HasForeignKey(s => s.StudentId)
                   .OnDelete(DeleteBehavior.SetNull);

            builder.Property(s => s.Email).HasMaxLength(256);
            builder.Property(s => s.TicketType)
                   .HasConversion<int>()
                   .HasDefaultValue(StudentSupportType.Other);

            builder.HasQueryFilter(s => !s.IsDeleted);
        }
    }
}
