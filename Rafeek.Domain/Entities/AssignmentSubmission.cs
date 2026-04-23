using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AssignmentSubmission : BaseEntity
    {
        public Guid AssignmentId { get; set; }
        public Assignment Assignment { get; set; } = null!;

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;

        public string SubmissionUrl { get; set; } = null!;
        public string? Feedback { get; set; }
        public float? Score { get; set; }
        public DateTime SubmittedAt { get; set; } = DateTime.UtcNow;
    }
}
