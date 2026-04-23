using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class Assignment : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public float TotalScore { get; set; }
        public bool IsActive { get; set; } = true;

        public Guid SectionId { get; set; }
        public Section Section { get; set; } = null!;

        public ICollection<AssignmentSubmission> Submissions { get; set; } = new HashSet<AssignmentSubmission>();
    }
}
