using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Section : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid InstructorId { get; set; }
        public Instructor Instructor { get; set; } = null!;

        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int Capacity { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
    }
}
