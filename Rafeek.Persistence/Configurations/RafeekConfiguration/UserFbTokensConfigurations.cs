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

            builder.HasOne(x => x.ApplicationUser)
                .WithMany(x => x.UserFbTokens)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
