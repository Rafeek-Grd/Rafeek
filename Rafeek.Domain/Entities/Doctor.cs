using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public ICollection<UserFbTokens> UserFbTokens { get; set; } = new List<UserFbTokens>();
    }
}
