using Rafeek.Application.Handlers.AuthHandlers;
using System.Security.Claims;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IJwtTokenManager
    {
        Task<AuthResult> GenerateClaimsTokenAsync(string email, CancellationToken cancellationToken = new CancellationToken());
        Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token, CancellationToken cancellationToken = new CancellationToken());
    }
}
