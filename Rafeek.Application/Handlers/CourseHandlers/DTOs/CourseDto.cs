namespace Rafeek.Application.Handlers.CourseHandlers.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public List<SectionDto> Sections { get; set; } = new List<SectionDto>();
    }
}
