using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Common.Interfaces;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class ApplicationUser : IdentityUser<Guid>
    {
        public string FullName { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public string? Code { get; set; }
        public string? Address { get; set; }
        public string? ImageName { get; set; }
        public string? BirthDate { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? ResetTokenExpireTime { get; set; }
        public int UserType { get; set; }
        public GenderType? Gender { get; set; }
        public int? Locked { get; set; }
        public ApplicationLanguage? Language { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string CreatedBy { get; set; } = null!;
        public string? UpdatedBy { get; set; }
        public string? DeletedBy { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }

        public ApplicationUser()
        {
            Id = Guid.NewGuid();
            CreatedAt = DateTime.Now;
            IsActive = true;
            IsDeleted = false;
        }
        public ICollection<UserFbTokens> UserFbTokens { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public Guid? SupervisorId { get; set; }
        public ApplicationUser? Supervisor { get; set; }
        public string? Speciality { get; set; }
        public string? LicenseNumber { get; set; }
    }
}
