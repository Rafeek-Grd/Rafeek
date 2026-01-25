namespace Rafeek.Infrastructure.Oauth
{
    public class JwtSettings
    {
        public string Secret { get; set; } = null!;
        public TimeSpan AccessTokenExpiration { get; set; }
        public bool ValidateIssuerSigningKey { get; set; }
        public bool ValidateIssuer { get; set; }
        public string Issuer { get; set; } = null!;
        public bool ValidateAudience { get; set; }
        public string Audience { get; set; } = null!;
        public bool RequireExpirationTime { get; set; }
        public bool ValidateLifetime { get; set; }
        public int RefreshTokenLifetime { get; set; }
        public TimeSpan RefreshTokenExpiration { get; set; }
    }
}
