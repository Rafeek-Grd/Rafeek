namespace Rafeek.Application.Handlers.AssignmentHandlers.DTOs
{
    public class AssignmentSubmissionDto
    {
        public Guid Id { get; set; }
        public Guid AssignmentId { get; set; }
        public Guid StudentId { get; set; }
        public string SubmissionUrl { get; set; } = null!;
        public string? Feedback { get; set; }
        public float? Score { get; set; }
        public DateTime SubmittedAt { get; set; }
    }
}
