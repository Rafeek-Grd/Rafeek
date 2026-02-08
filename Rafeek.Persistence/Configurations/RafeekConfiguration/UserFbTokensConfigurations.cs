using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class UserFbTokensConfigurations : IEntityTypeConfiguration<UserFbTokens>
    {
        public void Configure(EntityTypeBuilder<UserFbTokens> builder)
        {
            builder.ToTable("UserFbTokens");

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
