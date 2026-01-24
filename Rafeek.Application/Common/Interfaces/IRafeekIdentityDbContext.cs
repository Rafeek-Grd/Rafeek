using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IRafeekIdentityDbContext : IDisposable
    {
        public DbSet<UserFbTokens> FbTokens { get; set; }
        public DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
