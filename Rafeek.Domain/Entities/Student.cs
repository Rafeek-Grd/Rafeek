using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Student : BaseEntity
    {
        public string FullName { get; set; } = null!;
        public string NationalId { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        public int Level { get; set; }
        public int Term { get; set; }
        public string Status { get; set; } = null!;
    }
}
