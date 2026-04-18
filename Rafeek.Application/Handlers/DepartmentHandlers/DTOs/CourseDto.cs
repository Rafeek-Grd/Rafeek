namespace Rafeek.Application.Handlers.DepartmentHandlers.DTOs
{
    public class CourseDto
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public int CreditHours { get; set; }
        public string? Description { get; set; }
    }
}
