using Microsoft.AspNetCore.Identity;

namespace Rafeek.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string? Address { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiredTime { get; set; }
        public ICollection<Notification> Notifications { get; set; } = new HashSet<Notification>();
        public ICollection<UserLoginHistory> LoginHistories { get; set; } = new HashSet<UserLoginHistory>();

        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
