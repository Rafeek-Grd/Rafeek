using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Common;
using Rafeek.Domain.Entities;

namespace Rafeek.Persistence
{
    public class RafeekIdentityDbContext : IdentityDbContext<IdentityUser<Guid>, IdentityRole<Guid>, Guid,
        IdentityUserClaim<Guid>, IdentityUserRole<Guid>, IdentityUserLogin<Guid>,
        IdentityRoleClaim<Guid>, IdentityUserToken<Guid>>, IRafeekIdentityDbContext
    {
        private readonly ICurrentUserService? _currentUserService;

        public RafeekIdentityDbContext(DbContextOptions<RafeekIdentityDbContext> options) : base(options)
        {
        }

        public RafeekIdentityDbContext(DbContextOptions<RafeekIdentityDbContext> options,
            ICurrentUserService currentUserService) : base(options)
        {
            _currentUserService = currentUserService;
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ApplyConfigurationsFromAssembly(typeof(RafeekIdentityDbContext).Assembly,
                type => type.Namespace != null && type.Namespace.EndsWith("Configurations.IdentityConfiguration"));

            builder.HasDefaultSchema("auth");

            builder.Ignore<Department>();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(warnings =>
            {

                warnings.Ignore(CoreEventId.InvalidIncludePathError);
            });
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            foreach (var entry in ChangeTracker.Entries<BaseEntity>())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.Now;
                        entry.Entity.CreatedBy = !string.IsNullOrEmpty(entry.Entity.CreatedBy)
                            ? entry.Entity.CreatedBy
                            : (_currentUserService?.UserId.ToString() ?? "System");
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.Now;
                        entry.Entity.UpdatedBy = _currentUserService?.UserId.ToString() ?? "System";
                        break;
                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.Entity.DeletedAt = DateTime.Now;
                        entry.Entity.DeletedBy = _currentUserService?.UserId.ToString() ?? "System";
                        entry.Entity.IsDeleted = true;
                        entry.State = EntityState.Modified;
                        break;
                }
            }

            return await base.SaveChangesAsync(cancellationToken);
        }
    }
}
