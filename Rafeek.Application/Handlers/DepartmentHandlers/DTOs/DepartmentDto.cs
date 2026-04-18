namespace Rafeek.Application.Handlers.DepartmentHandlers.DTOs
{
    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Code { get; set; }
        public string? Description { get; set; }
    }
}
