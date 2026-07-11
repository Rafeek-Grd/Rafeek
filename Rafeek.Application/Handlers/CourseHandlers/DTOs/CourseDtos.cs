using Rafeek.Application.Handlers.CourseSectionHandlers.DTOs;

namespace Rafeek.Application.Handlers.CourseHandlers.DTOs
{
    // â”€â”€â”€ List DTO (Ø§Ù„Ø´Ø§Ø´Ø© Ø§Ù„Ø£ÙˆÙ„Ù‰ - Ø¨Ø·Ø§Ù‚Ø© Ø§Ù„Ù…Ù‚Ø±Ø± ÙÙŠ Ø§Ù„Ù‚Ø§Ø¦Ù…Ø©) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public class CourseListItemDto
    {
        public Guid CourseId { get; set; }

        /// <summary>Ø±Ù…Ø² Ø§Ù„Ù…Ù‚Ø±Ø± Ù…Ø«Ù„ CS-102</summary>
        public string Code { get; set; } = null!;

        public string Title { get; set; } = null!;
        public int CreditHours { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }

        /// <summary>Ø§Ø³Ù… Ù…Ø¯Ø±Ø³ Ø§Ù„Ù…Ø§Ø¯Ø© Ø§Ù„Ø±Ø¦ÙŠØ³ÙŠ</summary>
        public string? InstructorName { get; set; }

        /// <summary>Ø¹Ø¯Ø¯ Ø§Ù„Ø·Ù„Ø§Ø¨ Ø§Ù„Ù…Ø³Ø¬Ù„ÙŠÙ† Ø­Ø§Ù„ÙŠØ§Ù‹</summary>
        public int EnrolledStudents { get; set; }

        /// <summary>Ø§Ù„Ø·Ø§Ù‚Ø© Ø§Ù„Ø§Ø³ØªÙŠØ¹Ø§Ø¨ÙŠØ© Ø§Ù„Ù‚ØµÙˆÙ‰ (ÙŠÙØ­Ø¯ÙŽÙ‘Ø¯ Ù…Ù† LectureGroup.Capacity)</summary>
        public int Capacity { get; set; }

        /// <summary>
        /// Ø­Ø§Ù„Ø© Ø§Ù„Ù…Ù‚Ø±Ø± â€” Ø§Ù„Ù‚ÙŠÙ…: "Open" | "Full" | "Closed"
        /// Ø§Ù„ÙˆØ§Ø¬Ù‡Ø© ØªØ¹Ø±Ø¶Ù‡Ø§: Ù…ØªØ§Ø­ / Ù…ÙƒØªÙ…Ù„ / Ø¥Ù„ØºØ§Ø¡ Ø§Ù„ØªØ³Ø¬ÙŠÙ„
        /// </summary>
        public string Status { get; set; } = null!;
        public string StatusLabel { get; set; } = null!;

        /// <summary>Ø§Ù„Ù…ØªØ·Ù„Ø¨Ø§Øª Ø§Ù„Ø£Ø³Ø§Ø³ÙŠØ© (Ø±Ù…ÙˆØ² Ø§Ù„Ù…Ù‚Ø±Ø±Ø§Øª ÙÙ‚Ø· Ù„Ù„Ø¨Ø·Ø§Ù‚Ø©)</summary>
        public List<string> PrerequisiteCodes { get; set; } = new();

        /// <summary>Ø§Ø³Ù… Ø§Ù„ÙØµÙ„ Ø§Ù„Ø¯Ø±Ø§Ø³ÙŠ Ø§Ù„Ù…Ø±Ø¨ÙˆØ· (Ù…Ø«Ø§Ù„: Fall 2024)</summary>
        public string? AcademicTerm { get; set; }

        /// <summary>Ø´Ø¹Ø¨ Ø§Ù„Ù…Ù‚Ø±Ø±</summary>
        public List<CourseSectionDto> Sections { get; set; } = new();
    }

    // â”€â”€â”€ Detail DTO (Ø§Ù„Ø´Ø§Ø´Ø© Ø§Ù„Ø«Ø§Ù†ÙŠØ© - ØªÙØ§ØµÙŠÙ„ Ù…Ù‚Ø±Ø± ÙˆØ§Ø­Ø¯) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
    public class CourseDetailDto
    {
        public Guid CourseId { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public string? DepartmentName { get; set; }
        public Guid? DepartmentId { get; set; }

        // â”€â”€ Ø­Ø§Ù„Ø© Ø§Ù„ØªØ³Ø¬ÙŠÙ„ â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        /// <summary>"Open" | "Full" | "Closed"</summary>
        public string RegistrationStatus { get; set; } = null!;
        public string RegistrationStatusLabel { get; set; } = null!;
        public int EnrolledStudents { get; set; }
        public int Capacity { get; set; }

        /// <summary>ØªØ§Ø±ÙŠØ® Ø¨Ø¯Ø§ÙŠØ© Ø§Ù„Ù…Ù‚Ø±Ø±</summary>
        public DateTime? StartDate { get; set; }

        /// <summary>ØªØ§Ø±ÙŠØ® ÙØªØ­ Ø§Ù„ØªØ³Ø¬ÙŠÙ„</summary>
        public DateTime? RegistrationOpenDate { get; set; }

        /// <summary>ØªØ§Ø±ÙŠØ® Ø¥ØºÙ„Ø§Ù‚ Ø§Ù„ØªØ³Ø¬ÙŠÙ„</summary>
        public DateTime? RegistrationCloseDate { get; set; }

        /// <summary>Ø§Ù„ÙØµÙ„ Ø§Ù„Ø¯Ø±Ø§Ø³ÙŠ Ø§Ù„Ù…Ø±Ø¨ÙˆØ· Ø¨Ø§Ù„Ù…Ù‚Ø±Ø±</summary>
        public string? AcademicTerm { get; set; }

        // â”€â”€ Ù†ÙˆØ¹ Ø§Ù„Ù…Ù‚Ø±Ø± (ØªØ§Ø¬ Ù†Ø¸Ø±ÙŠ / ØªØ·Ø¨ÙŠÙ‚ÙŠ) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public bool IsTheoretical { get; set; }
        public bool IsPractical { get; set; }

        // â”€â”€ Ø§Ù„Ù…Ø³ØªÙˆÙ‰ Ø§Ù„Ø£ÙƒØ§Ø¯ÙŠÙ…ÙŠ Ø§Ù„Ù…Ø³ØªÙ‡Ø¯Ù â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public int TargetLevel { get; set; }

        // â”€â”€ Ø§Ù„Ù…ØªØ·Ù„Ø¨Ø§Øª Ø§Ù„Ø£Ø³Ø§Ø³ÙŠØ© â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public List<PrerequisiteStatusDto> Prerequisites { get; set; } = new();

        // â”€â”€ Ø§Ù„Ø®Ø·Ø© Ø§Ù„Ø¯Ø±Ø§Ø³ÙŠØ© (ØªÙˆØ²ÙŠØ¹ Ø§Ù„Ø³Ø§Ø¹Ø§Øª) â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public StudyPlanDistributionDto StudyPlanDistribution { get; set; } = new();
        public WeeklyHoursDto WeeklyHours { get; set; } = new();
        public GradeDistributionDto GradeDistribution { get; set; } = new();

        // â”€â”€ Ù…Ø¯Ø±Ø³Ùˆ Ø§Ù„Ù…Ø§Ø¯Ø© â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public List<CourseInstructorDto> Doctors { get; set; } = new();

        // â”€â”€ Ø´ÙØ¹Ø¨ Ø§Ù„Ù…Ø­Ø§Ø¶Ø±Ø§Øª ÙˆØ§Ù„Ù…ÙˆØ§Ø¹ÙŠØ¯ â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public List<LectureGroupDto> LectureGroups { get; set; } = new();

        public List<CourseSectionDto> Sections { get; set; } = new();
        // â”€â”€ Ø¥Ø´Ø¹Ø§Ø±Ø§Øª Ø®Ø§ØµØ© Ø¨Ø§Ù„Ù…Ù‚Ø±Ø± â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€â”€
        public List<CourseNotificationDto> Notifications { get; set; } = new();
    }

    public class PrerequisiteStatusDto
    {
        public Guid CourseId { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;

        /// <summary>
        /// Ø­Ø§Ù„Ø© Ø§Ù„Ù…ØªØ·Ù„Ø¨ Ù„Ù„Ø·Ø§Ù„Ø¨ â€” Ø§Ù„Ù‚ÙŠÙ…: "Met" | "InProgress" | "NotMet"
        /// Ø§Ù„ÙˆØ§Ø¬Ù‡Ø© ØªØ¹Ø±Ø¶Ù‡Ø§: Ø£Ø®Ø¶Ø± / Ø£ØµÙØ± / Ø£Ø­Ù…Ø±
        /// </summary>
        public string StudentStatus { get; set; } = "NotMet";
        public string StudentStatusLabel { get; set; } = "ØºÙŠØ± Ù…ÙƒØªÙ…Ù„";
    }

    public class StudyPlanDistributionDto
    {
        /// <summary>Ø³Ø§Ø¹Ø§Øª Ø§Ù„Ù…ØªØ·Ù„Ø¨Ø§Øª Ø§Ù„Ø¬Ø§Ù…Ø¹ÙŠØ©</summary>
        public int UniversityRequirementHours { get; set; }

        /// <summary>Ø³Ø§Ø¹Ø§Øª Ø§Ù„Ù…ØªØ·Ù„Ø¨Ø§Øª Ø§Ù„ØªØ®ØµØµÙŠØ©</summary>
        public int MajorRequirementHours { get; set; }

        /// <summary>Ø³Ø§Ø¹Ø§Øª Ø§Ù„Ù…Ù‚Ø±Ø±Ø§Øª Ø§Ù„Ø§Ø®ØªÙŠØ§Ø±ÙŠØ©</summary>
        public int ElectiveHours { get; set; }

        public float UniversityRequirementPercentage { get; set; }
        public float MajorRequirementPercentage { get; set; }
        public float ElectivePercentage { get; set; }
    }

    public class WeeklyHoursDto
    {
        public int LectureHours { get; set; }
        public int LabHours { get; set; }
    }

    public class GradeDistributionDto
    {
        public int MidtermPercent { get; set; }
        public int FinalPercent { get; set; }
        public int ProjectPercent { get; set; }
    }

    public class CourseInstructorDto
    {
        public Guid InstructorId { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? ProfilePictureUrl { get; set; }

        /// <summary>Ø³Ø§Ø¹Ø§Øª Ø§Ù„Ù…ÙƒØªØ¨</summary>
        public string? OfficeHours { get; set; }
    }

    public class CourseNotificationDto
    {
        public Guid NotificationId { get; set; }
        public string Title { get; set; } = null!;
        public string Message { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}


