namespace Rafeek.Application.Handlers.AdminHandlers.Queries.GetStudentProfile
{
    public class AdminStudentProfileDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string UniversityCode { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public int Level { get; set; }
        public string LevelName { get; set; } = null!;
        public string? AcademicAdvisorName { get; set; }
        public string? ProfilePictureUrl { get; set; }

        public List<AdminStudentCurrentEnrollmentDto> CurrentEnrollments { get; set; } = new();
        public List<AdminStudentResultHistoryDto> ResultsHistory { get; set; } = new();
    }

    public class AdminStudentCurrentEnrollmentDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string? InstructorName { get; set; }
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;
    }

    public class AdminStudentResultHistoryDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string SemesterName { get; set; } = null!;
        public float? Score { get; set; }
        public string? Grade { get; set; }
    }
}
