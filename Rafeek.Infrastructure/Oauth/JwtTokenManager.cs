using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AuthHandlers;
using Rafeek.Domain.Entities;
using Rafeek.Infrastructure.Oauth;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Squeak.Infrastructure.Oauth
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtTokenManager(JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, TokenValidationParameters tokenValidationParameters)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
        }

        public async Task<AuthResult> GenerateClaimsTokenAsync(string email, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.OutboundClaimTypeMap.Clear();

            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var currentTime = DateTimeOffset.UtcNow;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, currentTime.ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, currentTime.Add(_jwtSettings.AccessTokenExpiration).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = currentTime.Add(_jwtSettings.AccessTokenExpiration).UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                TokenType = "Bearer"
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                    new Claim(JwtRegisteredClaimNames.NameId, user.Id.ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, _jwtSettings.Issuer),
                    new Claim(JwtRegisteredClaimNames.Iat, currentTime.ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Nbf, currentTime.ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Exp, currentTime.Add(_jwtSettings.RefreshTokenExpiration).ToUnixTimeSeconds().ToString()),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                }),
                Expires = currentTime.Add(_jwtSettings.RefreshTokenExpiration).UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                TokenType = "Bearer"
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return new AuthResult
            {
                Token = tokenHandler.WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = token.ValidTo,
                RefreshToken = tokenHandler.WriteToken(refreshToken)
            };
        }

        public async Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token, CancellationToken cancellationToken = new CancellationToken())
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            tokenHandler.InboundClaimTypeMap.Clear();

            try
            {
                var tokenValdationParams = _tokenValidationParameters.Clone();
                tokenValdationParams.ValidateLifetime = false;

                tokenHandler.ValidateToken(token, tokenValdationParams, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                var jwtToken = (JwtSecurityToken)validatedToken;
                var identity = new ClaimsIdentity(jwtToken.Claims);
                return await Task.Run(() => new ClaimsPrincipal(identity));
            }
            catch
            {
                return null;
            }
        }

        private static bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
        {
            return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
                jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase);
        }
    }
}