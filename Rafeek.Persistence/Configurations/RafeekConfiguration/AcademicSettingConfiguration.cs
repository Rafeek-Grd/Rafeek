using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class AcademicSettingConfiguration : IEntityTypeConfiguration<AcademicSetting>
    {
        public void Configure(EntityTypeBuilder<AcademicSetting> builder)
        {
            builder.HasKey(x => x.Id);
            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
