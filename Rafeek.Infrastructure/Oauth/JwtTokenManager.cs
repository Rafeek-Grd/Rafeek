using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AuthHandlers;
using Rafeek.Domain.Entities;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Rafeek.Infrastructure.Oauth
{
    public class JwtTokenManager : IJwtTokenManager
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly TokenValidationParameters _tokenValidationParameters;
        private readonly IDataEncryption _dataEncryption;

        public JwtTokenManager(JwtSettings jwtSettings, UserManager<ApplicationUser> userManager, TokenValidationParameters tokenValidationParameters, IDataEncryption dataEncryption)
        {
            _jwtSettings = jwtSettings;
            _userManager = userManager;
            _tokenValidationParameters = tokenValidationParameters;
            _dataEncryption = dataEncryption;
        }

        public async Task<AuthResult> GenerateClaimsTokenAsync(string email, CancellationToken cancellationToken = new CancellationToken())
        {
            var user = await _userManager.FindByEmailAsync(email);

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

            var currentTime = DateTimeOffset.UtcNow;

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, _dataEncryption.Encrypt(user.Id.ToString())), // Encrypt User Id
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Nbf, currentTime.ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Exp, currentTime.Add(_jwtSettings.AccessTokenExpiration).ToUnixTimeSeconds().ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = currentTime.Add(_jwtSettings.AccessTokenExpiration).UtcDateTime,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Issuer = _jwtSettings.Issuer,
                TokenType = "Bearer"
            };

            var refreshTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, user.UserName),
                    new Claim(ClaimTypes.NameIdentifier, _dataEncryption.Encrypt(user.Id.ToString())), // Encrypt User Id
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

            // Create JWT tokens
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var refreshToken = tokenHandler.CreateToken(refreshTokenDescriptor);

            return new AuthResult
            {
                Token = tokenHandler.WriteToken(token),
                TokenType = "Bearer",
                ExpiresIn = token.ValidTo,
                RefreshToken = tokenHandler.WriteToken(refreshToken),
                RefreshTokenExpiration = refreshToken.ValidTo
            };
        }


        public async Task<ClaimsPrincipal> GetPrincipFromTokenAsync(string token, CancellationToken cancellationToken = new CancellationToken())
        {
            var tokenHandler = new JwtSecurityTokenHandler();

            try
            {
                // disable token lifetime validation as we are validating against an expired token.
                var tokenValdationParams = _tokenValidationParameters.Clone();
                tokenValdationParams.ValidateLifetime = false;

                var principal = tokenHandler.ValidateToken(token, tokenValdationParams, out var validatedToken);
                if (!IsJwtWithValidSecurityAlgorithm(validatedToken))
                {
                    return null;
                }

                return await Task.Run(() => principal);
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
