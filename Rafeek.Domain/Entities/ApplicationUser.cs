using Microsoft.AspNetCore.Identity;

namespace Rafeek.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? TemporaryEmail { get; set; }
        public bool IsUniversityEmailActivated { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Address { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiredTime { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<UserLoginHistory> LoginHistories { get; set; } = new HashSet<UserLoginHistory>();
        public ICollection<UserFbTokens> UserFbTokens { get; set; } = new List<UserFbTokens>();

        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
