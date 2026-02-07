using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfigurations
{
    public class StudentAcademicProfileConfiguration : IEntityTypeConfiguration<StudentAcademicProfile>
    {
        public void Configure(EntityTypeBuilder<StudentAcademicProfile> builder)
        {
            builder.ToTable("StudentAcademicProfiles");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.GPA)
                .HasColumnType("decimal(3,2)")
                .IsRequired();

            builder.Property(x => x.CGPA)
                .HasColumnType("decimal(3,2)")
                .IsRequired();

            builder.Property(x => x.CompletedCredits)
                .IsRequired();

            builder.Property(x => x.RemainingCredits)
                .IsRequired();

            builder.Property(x => x.Standing)
                .HasMaxLength(50)
                .IsRequired();

        
            builder.HasOne(x => x.Student)
                .WithOne(x => x.AcademicProfile)
                .HasForeignKey<StudentAcademicProfile>(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

         
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
