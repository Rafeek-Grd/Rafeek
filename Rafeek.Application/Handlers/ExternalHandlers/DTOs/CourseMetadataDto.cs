namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class CourseMetadataDto
    {
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Category { get; set; }
    }
}
