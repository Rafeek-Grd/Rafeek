using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Enrollment : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public Guid SectionId { get; set; }
        public Section Section { get; set; } = null!;

        public string Status { get; set; } = null!;
        public string? Grade { get; set; }
        public ICollection<Grade> Grades { get; set; } = new HashSet<Grade>();
    }
}
