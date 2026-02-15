using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class LearningResource : BaseEntity
    {
        public Guid CourseId { get; set; }
        public ResourceType ResourceType { get; set; }
        public string? ResourceUrl { get; set; } = null!;
        public string? Description { get; set; } = null!;

        public Course Course { get; set; } = null!;
    }
}
