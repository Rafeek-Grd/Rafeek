using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.Identity
{
    public class UserFbTokensConfigurations : IEntityTypeConfiguration<UserFbTokens>
    {
        public void Configure(EntityTypeBuilder<UserFbTokens> builder)
        {
            builder.ToTable("UserFbTokens");

            builder.HasQueryFilter(x => !x.IsDeleted);

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.UserFbTokens)
                .HasForeignKey(x => x.ApplicationUserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
