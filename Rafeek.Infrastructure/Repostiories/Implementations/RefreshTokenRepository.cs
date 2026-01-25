using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{

    internal class RefreshTokenRepository : BaseIdentityEntityRepository<RefreshToken, string>, IRefreshTokenRepository
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;

        public RefreshTokenRepository
        (
            IRafeekIdentityDbContext context,
            IJwtTokenManager jwtTokenManager,
            ICurrentUserService currentUserService) : base(context)
        {
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
        }

        public async Task<RefreshToken> GetToken(string token, CancellationToken cancellationToken)
        {
            try
            {
                var jwtToken = await _jwtTokenManager.GetPrincipFromTokenAsync(token);

                // Extract individual claims from the JWT
                string username = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
                string userId = jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
                string issuer = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss)?.Value;
                long issuedAt = Convert.ToInt64(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value);
                long notBefore = Convert.ToInt64(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Nbf)?.Value);
                long expiration = Convert.ToInt64(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value);
                string jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                return new RefreshToken()
                {
                    CreationDate = DateTimeOffset.FromUnixTimeSeconds(notBefore).LocalDateTime,
                    ExpirationDate = DateTimeOffset.FromUnixTimeSeconds(expiration).LocalDateTime,
                    JwtId = jti,
                    RemoteIpAddress = _currentUserService.IpAddress,
                    Token = token,
                    Revoked = null,
                    UserId = userId
                };
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<object> GenerateTokens(ApplicationUser user, CancellationToken cancellationToken)
        {
            try
            {
                var jwtToken = await _jwtTokenManager.GenerateClaimsTokenAsync(user.Email, cancellationToken);
                var lastToken = GetBy(x => x.UserId == user.Id.ToString() && x.RemoteIpAddress == _currentUserService.IpAddress).OrderBy(x => x.CreationDate).LastOrDefault();
                if (lastToken != null && lastToken.IsActive)
                {
                    Delete(lastToken);
                }

                var newToken = await GetToken(jwtToken.RefreshToken, cancellationToken);
                await AddAsync(newToken, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);
                // Transaction management delegated to IIdentityUnitOfWork - caller is responsible for SaveChangesAsync
                return jwtToken;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(LocalizationKeys.TokenMessages.NotValid.Value);
            }
        }
    }
}
