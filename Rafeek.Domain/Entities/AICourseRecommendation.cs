using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AICourseRecommendation : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public Guid CourseId { get; set; }
        public Course Course { get; set; } = null!;

        public string Reason { get; set; } = null!;

    }
}
