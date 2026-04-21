namespace Rafeek.Application.Handlers.DocumentHandlers.DTOs
{
    public class DocumentRequestDto
    {
        public Guid Id { get; set; }
        public string DocumentType { get; set; } = null!;
        public string Status { get; set; } = null!; // Pending, Approved, Rejected, Completed
        public string? Remarks { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
