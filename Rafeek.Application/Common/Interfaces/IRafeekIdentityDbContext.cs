using System;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IRafeekIdentityDbContext : IAsyncDisposable
    {
        DbSet<UserFbTokens> FbTokens { get; set; }
        DbSet<RefreshToken> RefreshTokens { get; set; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
