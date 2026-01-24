using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Student : BaseEntity
    {
        public Guid UserId { get; set; }
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
