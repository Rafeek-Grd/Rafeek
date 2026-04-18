namespace Rafeek.Application.Handlers.DepartmentHandlers.DTOs
{
    public class DepartmentUserDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string Role { get; set; } = null!; // Student, Doctor, Instructor
        public string? UniversityCode { get; set; }
    }
}
