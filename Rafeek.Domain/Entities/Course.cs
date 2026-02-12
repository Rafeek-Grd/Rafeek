using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Course : BaseEntity
    {
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public Guid DepartmentId { get; set; }
        public Department Department { get; set; } = null!;
        
        public ICollection<CoursePrerequisite> Prerequisites { get; set; } = new HashSet<CoursePrerequisite>();
        public ICollection<StudyPlan> StudyPlans { get; set; } = new HashSet<StudyPlan>();
        public ICollection<LearningResource> LearningResources { get; set; } = new HashSet<LearningResource>();
    }
}
