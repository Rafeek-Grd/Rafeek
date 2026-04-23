namespace Rafeek.Application.Handlers.InstructorHandlers.DTOs
{
    public class InstructorDashboardDto
    {
        public int TotalSections { get; set; }
        public int TotalStudents { get; set; }
        public int PendingAssignmentsToGrade { get; set; }
    }

    public class InstructorSectionDto
    {
        public Guid Id { get; set; }
        public string CourseTitle { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public int Capacity { get; set; }
        public int EnrolledCount { get; set; }
    }

    public class SectionStudentDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string UniversityCode { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string EnrollmentStatus { get; set; } = null!;
    }

    public class InstructorStudentProfileDto
    {
        public Guid StudentId { get; set; }
        public string FullName { get; set; } = null!;
        public string UniversityCode { get; set; } = null!;
        public string? ProfileImageUrl { get; set; }
        public string? Major { get; set; }
        public float? CGPA { get; set; }
    }

    public class InstructorExamScheduleDto
    {
        public string CourseTitle { get; set; } = null!;
        public string CourseCode { get; set; } = null!;
        public DateTime ExamDate { get; set; }
        public string Location { get; set; } = null!;
    }

    public class InstructorNotificationDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsRead { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class SectionGradeEntryDto
    {
        public Guid EnrollmentId { get; set; }
        public float Score { get; set; }
        public float TermGPA { get; set; }
        public float CGPA { get; set; }
    }
}
