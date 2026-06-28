using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class SecuritySettingConfiguration : IEntityTypeConfiguration<SecuritySetting>
    {
        public void Configure(EntityTypeBuilder<SecuritySetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
