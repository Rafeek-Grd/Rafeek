using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class CoursePrerequisite : BaseEntity
    {
        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;
        
        public Guid PrerequisiteId { get; set; }
        public Course Prerequisite { get; set; } = null!;
    }
}
