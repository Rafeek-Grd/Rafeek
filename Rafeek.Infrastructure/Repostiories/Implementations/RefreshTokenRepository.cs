using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;
using Rafeek.Persistence;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class RefreshTokenRepository : BaseEntityRepository<RefreshToken, Guid>, IRefreshTokenRepository
    {
        private readonly IJwtTokenManager _jwtTokenManager;
        private readonly ICurrentUserService _currentUserService;
        private readonly IRafeekDbContext? _context;
        private readonly IRafeekIdentityDbContext? _identityDbContext;

        public RefreshTokenRepository(IRafeekDbContext context, IJwtTokenManager jwtTokenManager, ICurrentUserService currentUserService) : base(context)
        {
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
            _context = context;
            _identityDbContext = null;
        }

        public RefreshTokenRepository(IRafeekIdentityDbContext identityDbContext, IJwtTokenManager jwtTokenManager, ICurrentUserService currentUserService) : base(identityDbContext)
        {
            _jwtTokenManager = jwtTokenManager;
            _currentUserService = currentUserService;
            _identityDbContext = identityDbContext;
            _context = null;
        }

        public async Task<RefreshToken> GetToken(string token, CancellationToken cancellationToken)
        {
            try
            {
                var jwtToken = await _jwtTokenManager.GetPrincipFromTokenAsync(token, cancellationToken);

                if (jwtToken == null)
                {
                    return null;
                }

                string username = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.UniqueName)?.Value
                               ?? jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;

                string userId = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.NameId)?.Value
                             ?? jwtToken.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;

                string issuer = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iss)?.Value;

                long.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Iat)?.Value, out long issuedAt);
                long.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Nbf)?.Value, out long notBefore);
                long.TryParse(jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Exp)?.Value, out long expiration);

                string jti = jwtToken.Claims.FirstOrDefault(x => x.Type == JwtRegisteredClaimNames.Jti)?.Value;

                return new RefreshToken()
                {
                    CreationDate = notBefore > 0 ? DateTimeOffset.FromUnixTimeSeconds(notBefore).LocalDateTime : DateTime.Now,
                    ExpirationDate = expiration > 0 ? DateTimeOffset.FromUnixTimeSeconds(expiration).LocalDateTime : DateTime.Now.AddDays(1),
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

                if (newToken == null)
                {
                    throw new UnauthorizedAccessException(LocalizationKeys.TokenMessages.NotValid.Value);
                }

                // Ensure UserId is set from the authenticated user if extraction failed
                if (string.IsNullOrEmpty(newToken.UserId))
                {
                    newToken.UserId = user.Id.ToString();
                }

                await AddAsync(newToken, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return jwtToken;
            }
            catch (Exception ex)
            {
                throw new UnauthorizedAccessException(LocalizationKeys.TokenMessages.NotValid.Value);
            }
        }

        public async Task<RefreshToken?> GetValidRefreshTokenAsync(string token, CancellationToken cancellationToken)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token && rt.Revoked == null, cancellationToken);

            if (refreshToken != null && !refreshToken.IsExpired && refreshToken.IsActive)
            {
                return refreshToken;
            }

            return null;
        }
    }
}