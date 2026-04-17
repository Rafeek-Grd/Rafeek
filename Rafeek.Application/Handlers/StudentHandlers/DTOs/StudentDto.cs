using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudentHandlers.DTOs
{
    public class StudentDto
    {
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string UniversityCode { get; set; } = null!;
        public DepartmentDto? Department { get; set; }
        public StudentStatus Status { get; set; }
        public int Level { get; set; }
        public int Term { get; set; }
        public AcademicProfileDto AcademicProfileDto { get; set; } = null!;
        public AdvisorDto? Advisor { get; set; }
    }

    public class DepartmentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
    }

    public class AcademicProfileDto
    {
        public Guid AcademicProfileId { get; set; }
        public float GPA { get; set; }
        public float CGPA { get; set; }
        public int CompletedCredits { get; set; }
        public int RemainingCredits { get; set; }
    }

    public class AdvisorDto
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public string? AdvisorName { get; set; }
        public string Email { get; set; } = null!;
        public string? EmployeeCode { get; set; }
        public DepartmentDto? Department { get; set; }

    }
}
