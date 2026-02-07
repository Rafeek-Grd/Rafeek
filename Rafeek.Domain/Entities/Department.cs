using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Department : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }

        public ICollection<ApplicationUser> Users { get; set; } = new HashSet<ApplicationUser>();
        public ICollection<Student> Students { get; set; } = new HashSet<Student>();
    }
}
