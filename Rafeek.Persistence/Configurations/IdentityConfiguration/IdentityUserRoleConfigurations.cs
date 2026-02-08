using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.IdentityConfiguration
{
    public class IdentityUserRoleConfigurations : IEntityTypeConfiguration<IdentityUserRole<Guid>>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<IdentityUserRole<Guid>> builder)
        {
            builder.ToTable("UserRoles");
        }
    }
}
