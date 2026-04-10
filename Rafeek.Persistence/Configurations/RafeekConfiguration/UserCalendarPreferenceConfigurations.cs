using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class UserCalendarPreferenceConfigurations : IEntityTypeConfiguration<UserCalendarPreference>
    {
        public void Configure(EntityTypeBuilder<UserCalendarPreference> builder)
        {
            builder.ToTable("UserCalendarPreferences");

            builder.HasOne(x => x.User)
                .WithOne(u => u.CalendarPreference)
                .HasForeignKey<UserCalendarPreference>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
