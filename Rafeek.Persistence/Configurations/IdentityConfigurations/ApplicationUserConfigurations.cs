using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Persistence.Configurations.Identity
{
    public class ApplicationUserConfigurations : IEntityTypeConfiguration<ApplicationUser>
    {

        public void Configure(EntityTypeBuilder<ApplicationUser> builder)
        {
            builder.HasDiscriminator(x => x.UserType)
              .HasValue<ApplicationUser>((int)UserType.Student)
              .HasValue<TeachingAssistant>((int)UserType.TeachingAssistant)
              .HasValue<Doctor>((int)UserType.Doctor);


            builder.Property(s => s.Language).HasDefaultValue(ApplicationLanguage.AR);
        }
    }
}
