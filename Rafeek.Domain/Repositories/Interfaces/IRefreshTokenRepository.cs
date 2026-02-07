using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Domain.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, Guid>
    {
        Task<RefreshToken> GetToken(string token, CancellationToken cancellationToken = default);
        Task<object> GenerateTokens(IdentityUser<Guid> user, CancellationToken cancellationToken = default);
    }
}
