using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Doctor: BaseEntity
    {
        public Guid UserId { get; set; }
        public string? Speciality { get; set; }
        public string? LicenseNumber { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
    }
}
