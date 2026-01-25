using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Domain.Repositories.Interfaces
{
    public interface IRefreshTokenRepository : IGenericRepository<RefreshToken, string>
    {
        Task<RefreshToken> GetToken(string token, CancellationToken cancellationToken = new CancellationToken());
        Task<object> GenerateTokens(ApplicationUser user, CancellationToken cancellationToken = new CancellationToken());
    }
}
