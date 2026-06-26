using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.GenericHandlers.GetProfilesForAdmins
{
    public class GetProfilesForAdminsResponse
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = null!;
        public string? Email { get; set; }
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? DepartmentName { get; set; }
        public List<UserType> UserTypes { get; set; } = new();

        public MentorProfileSection? Mentor { get; set; }
        public ProfessorProfileSection? Professor { get; set; }
        public StudentProfileSection? Student { get; set; }
        public StaffProfileSection? Staff { get; set; }
        public AdminProfileSection? Admin { get; set; }
    }

    public class MentorProfileSection
    {
        public string? AcademicTitle { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? Office { get; set; }
        public string? Biography { get; set; }
        public string? Status { get; set; }
        public List<OfficeHourDto> OfficeHours { get; set; } = new();
        public List<TeachingCourseDto> TeachingCourses { get; set; } = new();
        public List<AdvisedStudentDto> AdvisedStudents { get; set; } = new();
    }

    public class ProfessorProfileSection
    {
        public string? AcademicTitle { get; set; }
        public string? EmployeeNumber { get; set; }
        public string? Office { get; set; }
        public string? Biography { get; set; }
        public string? Status { get; set; }
        public List<OfficeHourDto> OfficeHours { get; set; } = new();
        public List<TeachingCourseDto> TeachingCourses { get; set; } = new();
    }

    public class StudentProfileSection
    {
        public string? UniversityCode { get; set; }
        public int Level { get; set; }
        public string? LevelName { get; set; }
        public string? AcademicAdvisorName { get; set; }
        public List<AdminStudentCurrentEnrollmentDto> CurrentEnrollments { get; set; } = new();
        public List<AdminStudentResultHistoryDto> ResultsHistory { get; set; } = new();
    }

    public class StaffProfileSection
    {
        public string? Title { get; set; }
        public string? EmployeeCode { get; set; }
        public List<OfficeHourDto> OfficeHours { get; set; } = new();
        public List<StaffCourseDto> TaughtCourses { get; set; } = new();
    }

    public class AdminProfileSection
    {
        public string? Role { get; set; }
    }

    public class OfficeHourDto
    {
        public string Day { get; set; } = null!;
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public string? Location { get; set; }
    }

    public class TeachingCourseDto
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseName { get; set; } = null!;
        public string? Semester { get; set; }
        public string? AcademicYear { get; set; }
        public int CreditHours { get; set; }
    }

    public class AssistantMentorDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string? Position { get; set; }
        public string? AssignedCourse { get; set; }
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

    public class StaffCourseDto
    {
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public string SemesterName { get; set; } = null!;
    }

    public class AdvisedStudentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Image { get; set; }
        public string UniversityCode { get; set; } = null!;
        public int Level { get; set; }
        public string? LevelName { get; set; }
        public string Status { get; set; } = null!;
    }
}
