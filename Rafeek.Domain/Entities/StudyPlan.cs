using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class StudyPlan : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Semester Semester { get; set; }
        public Guid CourseId { get; set; }

        public Student Student { get; set; } = null!;
        public Course Course { get; set; } = null!;
    }
}
