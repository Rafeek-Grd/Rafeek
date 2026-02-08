using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class IdentityUserConfigurations : IEntityTypeConfiguration<IdentityUser<Guid>>
    {
        public void Configure(EntityTypeBuilder<IdentityUser<Guid>> builder)
        {
            builder.ToTable("ApplicationUsers");
        }
    }
}
