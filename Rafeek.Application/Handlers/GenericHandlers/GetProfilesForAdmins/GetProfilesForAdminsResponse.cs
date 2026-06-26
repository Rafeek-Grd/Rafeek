namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public class GetProfilesForAdminsResponse
    {
        public IReadOnlyList<AdminStaffProfileDto>? StaffProfile { get; set; } = null!;
        public IReadOnlyList<AdminStudentProfileDto>? StudentProfile { get; set; } = null!;
    }

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

    public class AdminStaffProfileDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? DepartmentName { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public List<WorkingHourDto> WorkingHours { get; set; } = new();
        public List<CoTeacherDto> CoTeachers { get; set; } = new();
        public List<StaffCourseDto> TaughtCourses { get; set; } = new();
    }

    public class WorkingHourDto
    {
        public string Days { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Location { get; set; } = null!;
    }

    public class CoTeacherDto
    {
        public string Name { get; set; } = null!;
        public string RoleDesc { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }
    }

    public class StaffCourseDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string SemesterName { get; set; } = null!;
    }
}
