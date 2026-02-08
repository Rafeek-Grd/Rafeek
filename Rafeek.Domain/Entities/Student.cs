using Microsoft.AspNetCore.Identity;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int Level { get; set; }
        public int Term { get; set; }
        public string Status { get; set; } = null!;
        public Guid AcademicProfileId { get; set; }
        public StudentAcademicProfile? AcademicProfile { get; set; }
        public Guid UserId { get; set; }
        public IdentityUser<Guid> User { get; set; } = null!;
        public ICollection<UserFbTokens> UserFbTokens { get; set; } = new List<UserFbTokens>();
    }
}
