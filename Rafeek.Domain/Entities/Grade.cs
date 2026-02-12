using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Grade : BaseEntity
    {
        public Guid EnrollmentId { get; set; }
        public Enrollment Enrollment { get; set; } = null!;

        public float GradeValue { get; set; }
        public float TermGPA { get; set; }
        public float CGPA { get; set; }
    }
}
