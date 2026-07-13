using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class CourseSection : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public int Day { get; set; }
        public TimeSpan StartTime { get; set; }
        public int Duration { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; } = new HashSet<Enrollment>();
    }
}
