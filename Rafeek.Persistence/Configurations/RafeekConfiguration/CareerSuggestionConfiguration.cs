using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class CareerSuggestionConfiguration : IEntityTypeConfiguration<CareerSuggestion>
    {
        public void Configure(EntityTypeBuilder<CareerSuggestion> builder)
        {
            builder.ToTable(nameof(CareerSuggestion));

            builder.HasKey(x => x.Id);

            builder.Property(x => x.CareerPath)
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.Justification)
                .HasMaxLength(1000)
                .IsRequired();

            // Relations
            builder.HasOne(x => x.Student)
                .WithMany(x => x.CareerSuggestions)
                .HasForeignKey(x => x.StudentId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasIndex(x => x.StudentId);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
