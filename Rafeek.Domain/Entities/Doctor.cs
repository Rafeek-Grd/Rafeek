using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Doctor : BaseEntity
    {
        public string? EmployeeCode { get; set; }
        public Guid UserId { get; set; }
        public ApplicationUser User { get; set; } = null!;
        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }
        public bool IsAcademicAdvisor { get; set; }

        public ICollection<Student> AdvisedStudents { get; set; } = new HashSet<Student>();
        public ICollection<Appointment> Appointments { get; set; } = new HashSet<Appointment>();
    }
}
