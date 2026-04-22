namespace Rafeek.Application.Handlers.AssignmentHandlers.DTOs
{
    public class AssignmentDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public float TotalScore { get; set; }
        public bool IsActive { get; set; }
        public Guid SectionId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
