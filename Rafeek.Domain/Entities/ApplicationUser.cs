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

        public ApplicationUser()
        {
            Id = Guid.NewGuid();
        }
    }
}
