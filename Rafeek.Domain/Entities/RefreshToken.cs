using System.ComponentModel.DataAnnotations;

namespace Rafeek.Domain.Entities
{
    public class RefreshToken
    {
        [Key]
        public string JwtId { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public string Token { get; set; } = null!;
        public DateTime CreationDate { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpirationDate;
        public DateTime? Revoked { get; set; }
        public bool IsActive => Revoked == null && !IsExpired;
        public string RemoteIpAddress { get; set; } = null!;

        public void Revoke()
        {
            Revoked = DateTime.UtcNow;
        }
    }
}
