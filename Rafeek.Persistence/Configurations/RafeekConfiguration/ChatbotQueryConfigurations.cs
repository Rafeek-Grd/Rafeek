using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence.Configurations.RafeekConfiguration
{
    public class ChatbotQueryConfigurations : IEntityTypeConfiguration<ChatbotQuery>
    {
        public void Configure(EntityTypeBuilder<ChatbotQuery> builder)
        {
            builder.HasOne(chq => chq.Student)
                .WithMany(s => s.ChatbotQueries)
                .HasForeignKey(chq => chq.StudentId)
                .OnDelete(DeleteBehavior.Cascade);


            builder.HasQueryFilter(x => !x.IsDeleted);
        }
    }
}
