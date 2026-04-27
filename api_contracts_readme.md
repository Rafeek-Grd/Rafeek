# Rafeek API Contracts

This document contains all Enums, Request (Commands/Queries), and Response (DTOs) entities for the UI developers.

## 0. JSON Response Examples

### Basic Result
```json
{
  "succeeded": true,
  "mainResult": null,
  "data": { },
  "errors": []
}
```

### Paginated Result
```json
{
  "items": [ ],
  "pageNumber": 1,
  "pageSize": 20,
  "totalPages": 5,
  "totalCount": 100,
  "hasPreviousPage": false,
  "hasNextPage": true
}
```

### API Response
```json
{
  "success": true,
  "data": { },
  "message": "Operation successful",
  "statusCode": 200
}
```

## 1. Enums

```csharp
public enum AcademicCalendarEventType
    {
    General = 0,
    Academic = 1,
    Guidance = 2,
    OfficialHoliday = 3,
    Exam = 4,
    Deadline = 5
}

public enum AcademicServiceType
    {
    AcademicAdvisingAppointment = 1,
    ChangeMajor,
    CourseDropOrAdd,
    ExcuseFromExam,
    WithdrawalFromSemester,
    Other
}

public enum AcademicYearStatus
    {
    Draft = 0,
    Published = 1,
    Archived = 2
}

public enum ApplicationLanguage
    {
    AR = 0,
    EN = 1
}

public enum AppointmentStatus
    {
    Scheduled = 0,
    InProgress,
    Completed,
    Cancelled
}

public enum CalendarEventStatus
    {
    Draft = 0,
    Published = 1,
    Cancelled = 2
}

public enum DataProtectionPurpose
    {
    RouteValues
}

public enum DocumentStatus
    {
    Pending = 0,
    Approved = 1,
    Rejected = 2
}

public enum EmailType
    {
    Default,
    OTP
}

public enum EventVisibility
    {
    All = 0,
    Department = 1,
    Personal = 2
}

public enum Floor
    {
    A,
    B,
    C,
    D
}

public enum GenderType
    {
    Male = 1,
    Female = 2
}

public enum NotificationType
    {
    Academic = 0,
    Scheduled = 1,
    Announcement = 2
}

public enum RecurrenceType
    {
    None = 0,
    Daily = 1,
    Weekly = 2,
    Monthly = 3
}

public enum ReportType
    {
    AcademicPerformance = 1,
    AttendanceReport = 2,
    GradeAnalysis = 3,
    CourseCompletion = 4,
    SkillsAssessment = 5,
    CareerReadiness = 6,
    Custom = 99
}

public enum ResourceType
    {
    External = 0
}

public enum Semester
    {
    First = 0,
    Second,
    Third,
    Fourth,
    Fifth,
    Sixth,
    Seventh,
    Eighth
}

public enum Standing
    {
    Freshman = 0,
    Sophomore = 1,
    Junior = 2,
    Senior = 3,
    Graduate = 4
}

public enum StudentStatus
    {
    Active = 0,
    Inactive
}

public enum StudentSupportStatus
    {
    Pending =0,
    Approved,
    Rejected
}

public enum TermType
    {
    Fall = 0,
    Spring = 1,
    Summer = 2
}

public enum UserType
    {
    None = 0,
    Admin = 1,
    SubAdmin = 2,
    Staff = 4,
    Instructor = 8,
    Doctor = 16,
    Student = 32
}

```

## 2. Request & Response Entities

```csharp
    public record ApiResponse<TData>
    {
    public bool Success { get; set; }
    public TData? Data { get; set; }
    public string? Message { get; set; }
    public int StatusCode { get; set; }
        {
            {
        }
        {
            {
        }
        {
            {
        }
    }

    public class PagginatedResult<T>
    {
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }
    public bool HasPreviousPage => PageNumber > 1;
    public bool HasNextPage => PageNumber < TotalPages;
    }

    public class Result
    {
    public bool Succeeded { get; set; }
    public object MainResult { get; set; }
    public object Data { get; set; }
    public string[] Errors { get; set; }
        {
        }
    }

    public class AddEventToAcademicCalendarCommand: IRequest<string>
    {
    public string EventName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAllDay { get; set; }
    public string? Location { get; set; }
    public AcademicCalendarEventType EventType { get; set; } = AcademicCalendarEventType.General;
    public CalendarEventStatus Status { get; set; } = CalendarEventStatus.Draft;
    public EventVisibility Visibility { get; set; } = EventVisibility.All;
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
    public DateTime? RecurrenceEndDate { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? AcademicTermId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? SectionId { get; set; }
    }

    public class DeleteEventOfAcademicCalendarCommand: IRequest<string>
    {
    public string AcademicEventId { get; set; } = string.Empty;
    }

    public class UpdateEventOfAcademicCalendarCommand : IRequest<string>
    {
    public Guid Id { get; set; }
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public DateTime? EventDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan? StartTime { get; set; }
    public TimeSpan? EndTime { get; set; }
    public bool? IsAllDay { get; set; }
    public string? Location { get; set; }
    public AcademicCalendarEventType? EventType { get; set; }
    public CalendarEventStatus? Status { get; set; }
    public EventVisibility? Visibility { get; set; }
    public RecurrenceType? RecurrenceType { get; set; }
    public DateTime? RecurrenceEndDate { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? AcademicTermId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? SectionId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    }

    public record AcademicCalendarDto
    {
    public Guid Id { get; set; }
    public string? EventName { get; set; }
    public string? Description { get; set; }
    public DateTime EventDate { get; set; }
    public DateTime? EndDate { get; set; }
    public TimeSpan StartTime { get; set; }
    public TimeSpan EndTime { get; set; }
    public bool IsAllDay { get; set; }
    public string? Location { get; set; }
    public AcademicCalendarEventType EventType { get; set; }
    public CalendarEventStatus Status { get; set; }
    public EventVisibility Visibility { get; set; }
    public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
    public DateTime? RecurrenceEndDate { get; set; }
    public Guid? TargetUserId { get; set; }
    public Guid? AcademicTermId { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? CourseId { get; set; }
    public Guid? SectionId { get; set; }
    }

    public class GetAllEventsPagginatedQuery: IRequest<PagginatedResult<AcademicCalendarDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class GetEventOfAcademicCalendarByIdQuery: IRequest<AcademicCalendarDto>
    {
    public Guid AcademicCalendarId { get; set; }
    }

    public class CreateAcademicTermCommand: IRequest<Unit>
    {
    public string Name { get; set; } = null!;
    public TermType TermType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? RegistrationStartDate { get; set; }
    public DateTime? RegistrationEndDate { get; set; }
    public DateTime? DropDeadline { get; set; }
    public DateTime? ExamStartDate { get; set; }
    public DateTime? ExamEndDate { get; set; }
    public Guid AcademicYearId { get; set; }
    }

    public class DeleteAcademicTermCommand: IRequest<Unit>
    {
    public Guid Id { get; set; }
    }

    public class UpdateAcademicTermCommand: IRequest<Unit>
    {
    public Guid Id { get; set; }
    public string? Name { get; set; } = null!;
    public TermType? TermType { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? RegistrationStartDate { get; set; }
    public DateTime? RegistrationEndDate { get; set; }
    public DateTime? DropDeadline { get; set; }
    public DateTime? ExamStartDate { get; set; }
    public DateTime? ExamEndDate { get; set; }
    public Guid? AcademicYearId { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    }

    public class AcademicTermDto
    {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public TermType TermType { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime? RegistrationStartDate { get; set; }
    public DateTime? RegistrationEndDate { get; set; }
    public DateTime? DropDeadline { get; set; }
    public DateTime? ExamStartDate { get; set; }
    public DateTime? ExamEndDate { get; set; }
    public Guid AcademicYearId { get; set; }
    public AcademicYearDto AcademicYear { get; set; } = null!;
    }

    public class GetAllAcademicTermsQuery : IRequest<List<AcademicTermDto>>
    {
    }

    public class GetAllAcademicTermsQueryHandler : IRequestHandler<GetAllAcademicTermsQuery, List<AcademicTermDto>>
    {
        {
        }
        {
        }
    }

    public class GetAcademicTermByIdQuery: IRequest<AcademicTermDto>
    {
    public Guid Id { get; set; }
    }

    public class GetAllAcademicTermPagginatedQuery: IRequest<PagginatedResult<AcademicTermDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class AddAcademicYearCommand : IRequest<Unit>
    {
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentYear { get; set; }
    }

    public class DeleteAcademicYearCommand: IRequest<Unit>
    {
    public Guid Id { get; set; }
    }

    public class UpdateAcademicYearCommand: IRequest<Unit>
    {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool? IsCurrentYear { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    }

    public class AcademicYearDto
    {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public bool IsCurrentYear { get; set; }
    public IReadOnlyList<AcademicTermDto>? Terms { get; set; }
    }

    public class GetAcademicYearByIdQuery : IRequest<AcademicYearDto?>
    {
    public Guid Id { get; set; }
    }

    public class GetAllAcademicYearsPagginatedQuery : IRequest<PagginatedResult<AcademicYearDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class GetAcademicSchedulesQuery : IRequest<List<AcademicScheduleDto>>
    {
    public Guid? TermId { get; set; }
    }

    public class AcademicScheduleDto
    {
    public Guid SectionId { get; set; }
    public string CourseTitle { get; set; } = null!;
    public string CourseCode { get; set; } = null!;
    public string InstructorName { get; set; } = null!;
    public string Room { get; set; } = null!;
    public string Day { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
    }

    public class AdminDashboardDto
    {
    }

    public class AcademicLevelTrendDto
    {
    public float ChangePercentage { get; set; }
    }

    public class GpaTrendPointDto
    {
    public string Label { get; set; } = string.Empty;
    public float AverageGpa { get; set; }
    }

    public class BatchDistributionDto
    {
    }

    public class BatchSliceDto
    {
    public int Count { get; set; }
    public float Percentage { get; set; }
    }

    public class AcademicStatusAnalysisDto
    {
    }

    public class StatusSliceDto
    {
    public int Count { get; set; }
    public float Percentage { get; set; }
    }

    public class AcademicObstaclesDto
    {
    public int RegistrationHolds { get; set; }
    public int AcademicProbation { get; set; }
    public int MissingRequirements { get; set; }
    }

    public class GetAdminDashboardQuery : IRequest<AdminDashboardDto>
    {
    }

    public class GetExamResultsQuery : IRequest<List<ExamResultItemDto>>
    {
    public string? TabName { get; set; } // e.g. "AI", "SWE", "1st General"
    }

    public class ExamResultItemDto
    {
    public Guid EnrollmentId { get; set; }
    public string StudentCode { get; set; } = null!;
    public string StudentName { get; set; } = null!;
    public string StudentEmail { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public string LetterGrade { get; set; } = null!;
    public float Score { get; set; }
    public bool IsPublished { get; set; }
    }

    public class GetExamsScheduleQuery : IRequest<List<ExamDayGroupDto>>
    {
    public string? TermId { get; set; }
    public string? SearchText { get; set; }
    }

    public class ExamDayGroupDto
    {
    public DateTime Date { get; set; }
    public string FormattedDate { get; set; } = null!;
    public string ExamCountLabel { get; set; } = null!;
    }

    public class ExamItemDto
    {
    public Guid ExamId { get; set; }
    public string TimeLabel { get; set; } = null!;
    public string DurationLabel { get; set; } = null!;
    public string CourseCode { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public string Location { get; set; } = null!;
    public string TargetLevel { get; set; } = null!; // e.g. Undergrad Year 1
    }

    public class GetSecurityDashboardQuery : IRequest<SecurityDashboardDto> { }
    public class SecurityDashboardDto
    {
    public int NewUsersCount { get; set; }
    public int ActiveUsersCount { get; set; }
    public int TotalUsersCount { get; set; }
    }

    public class ChartItemDto
    {
    public string Label { get; set; } = null!;
    public int Percentage { get; set; }
    public string? ColorHex { get; set; }
    }

    public class SecurityPoliciesDto
    {
    public bool TwoFactorAuthEnabled { get; set; }
    public bool PasswordChangeForced { get; set; }
    public string SessionTimeout { get; set; } = null!;
    }

    public class SuspiciousIPDto
    {
    public string IP { get; set; } = null!;
    public string LocationInfo { get; set; } = null!;
    }

    public class LiveActivityDto
    {
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string TimeLabel { get; set; } = null!;
    public string TypeIcon { get; set; } = null!;
    }

    public class PermissionMatrixItemDto
    {
    public string ModuleName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public bool CanView { get; set; }
    public bool CanEdit { get; set; }
    public bool CanCreate { get; set; }
    public bool CanDelete { get; set; }
    public bool CanExport { get; set; }
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

    public class GetAdminStaffProfileQuery : IRequest<AdminStaffProfileDto>
    {
    public Guid UserId { get; set; }
    }

    public class GetStudentAcademicRecordsQuery : IRequest<PagginatedResult<StudentAcademicRecordDto>>
    {
    public string? AcademicStatus { get; set; }
    public string? SearchTerm { get; set; }
    public float? Cgpa { get; set; }
    public Guid? DepartmentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    }

    public class StudentAcademicRecordDto
    {
    public Guid StudentId { get; set; }
    public string FullName { get; set; } = null!;
    public string UniversityEmail { get; set; } = null!;
    public string UniversityCode { get; set; } = null!;
    public string? DepartmentName { get; set; }
    public float Cgpa { get; set; }
    public string AcademicStatus { get; set; } = null!;
    public string AcademicStatusLabel { get; set; } = null!;
    public int Level { get; set; }
    public int Term { get; set; }
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

    public class GetAdminStudentProfileQuery : IRequest<AdminStudentProfileDto>
    {
    public Guid StudentId { get; set; }
    }

    public class GetUserManagementQuery : IRequest<PagginatedResult<UserManagementListItemDto>>
    {
    public UserType TabRole { get; set; } = UserType.Student;
    public string? SearchTerm { get; set; }
    public string? Status { get; set; }
    public Guid? DepartmentId { get; set; }
    public bool? IsAdvisor { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    }

    public class UserManagementListItemDto
    {
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Role { get; set; } = null!;
    public string? DepartmentName { get; set; }
    public string Status { get; set; } = null!;
    public string StatusLabel { get; set; } = null!;
    public string IdentificationNumber { get; set; } = null!;
    }

    public class AssignStudentsToAcademicAdvisorCommand: IRequest<Unit>
    {
    public Guid AcademicAdvisorId { get; set; }
    public List<Guid> StudentIds { get; set; } = null!;
    }

    public class UpdateStatusOfGuidenceRequestCommand: IRequest<Unit>
    {
    public Guid AdvisorId { get; set; }
    public Guid RequestId { get; set; }
    public StudentSupportStatus Status { get; set; }
    }

    public class GuidanceRequestDto
    {
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public StudentSupportStatus StudentSupportStatus { get; set; }
    public Guid StudentId { get; set; }
    public StudentDto Student { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    }

    public class GetAllGuidenceSupportRequestsPagginatedQuery: IRequest<PagginatedResult<GuidanceRequestDto>>
    {
    public StudentSupportStatus? RequestStatus { get; set; }
    public string? SearchTerm { get; set; }
    public Guid? DepartmentId { get; set; }
    public Guid? AdvisorId { get; set; }
    public Guid? StudentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class AskAiCommand : IRequest<AiChatResponseDto>
    {
    public string Question { get; set; } = string.Empty;
    public Guid? SessionId { get; set; }
    public List<ChatMessageDto>? History { get; set; }
    }

    public class GenerateAITimetableCommand : IRequest<AITimetableResponseDto>
    {
    public AITimetableRequestDto TimetableRequest { get; set; } = null!;
    }

    public class SaveAITimetableCommand : IRequest<Guid>
    {
    public Guid? Id { get; set; }
    public Guid StudentId { get; set; }
    public string? TimetableName { get; set; }
    public AITimetableResponseDto TimetableData { get; set; } = null!;
    }

    public class AiChatResponseDto
    {
    public string Answer { get; set; } = string.Empty;
    public Guid SessionId { get; set; }
    }

    public class AiSessionDto
    {
    public Guid SessionId { get; set; }
    public string? SessionTitle { get; set; }
    public DateTime LastMessageAt { get; set; }
    public int MessagesCount { get; set; }
    }

    public class ChatHistoryDto
    {
    public Guid Id { get; set; }
    public Guid SessionId { get; set; }
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
    public DateTime AskedAt { get; set; }
    }

    public class ChatMessageDto
    {
    public string Role { get; set; } = string.Empty; // "user" or "ai" or "assistant"
    public string Content { get; set; } = string.Empty;
    }

    public class GetAICourseRecommendationsQuery : IRequest<AIRecommendationDto>
    {
    public Guid StudentId { get; set; }
    }

    public class GetAiSessionsQuery : IRequest<List<AiSessionDto>>
    {
    }

    public class GetChatHistoryQuery : IRequest<List<ChatHistoryDto>>
    {
    public Guid? SessionId { get; set; }
    public int Page { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class CreateAssignmentCommand : IRequest<Guid>
    {
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public float TotalScore { get; set; }
    public Guid SectionId { get; set; }
    }

    public class GradeAssignmentSubmissionCommand : IRequest<Unit>
    {
    public Guid SubmissionId { get; set; }
    public float Score { get; set; }
    public string? Feedback { get; set; }
    }

    public class AssignmentDto
    {
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime DueDate { get; set; }
    public float TotalScore { get; set; }
    public bool IsActive { get; set; }
    public Guid SectionId { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class AssignmentSubmissionDto
    {
    public Guid Id { get; set; }
    public Guid AssignmentId { get; set; }
    public Guid StudentId { get; set; }
    public string SubmissionUrl { get; set; } = null!;
    public string? Feedback { get; set; }
    public float? Score { get; set; }
    public DateTime SubmittedAt { get; set; }
    }

    public class AuthResult
    {
    public string Token { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public DateTime ExpiresIn { get; set; }
    public string RefreshToken { get; set; } = null!;
    }

    public class SignResponse
    {
    public string Id { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string FullName { get; set; } = null!;
    public string Phone { get; set; } = null!;
    public string? ProfilePictureUrl { get; set; }
    public string Token { get; set; } = null!;
    public string RefreshToken { get; set; } = null!;
    public string TokenType { get; set; } = null!;
    public object ExpiresIn { get; set; } = null!;
    }

    public class ActivateUniversityEmailCommand : IRequest<string>
    {
    public string Email { get; set; } = null!;
    public string ConfirmationCode { get; set; } = null!;
    }

    public class CheckFromConfirmationCodeResponse
    {
    public bool IsValid { get; set; }
    public string? Message { get; set; }
    }

    public class CheckFromConfirmationCodeCommand : IRequest<CheckFromConfirmationCodeResponse>
    {
    public string Email { get; set; } = string.Empty;
    public string ConfirmationCode { get; set; } = string.Empty;
    }

    public class ForegetPasswordCommand: IRequest<string>
    {
    public string Email { get; set; } = string.Empty;
    }

    public class RefreshTokenCommand : IRequest<AuthResult>
    {
    public string Token { get; set; } = null!;
    }

    public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
    {
    public string Email { get; set; } = null!;
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
    }

    public class ResetPasswordResponse
    {
    public string Message { get; set; } = null!;
    }

    public class SendUserCredentialsCommand : IRequest<string>
    {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string ConfirmationCode { get; set; } = null!;
    }

    public class SignInCommand : IRequest<SignResponse>
    {
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;
    public string? FbToken { get; set; }
    public bool IsAndroidDevice { get; set; }
    public bool IsIosDevice { get; set; }
    }

    public class SignUpCommand : IRequest<SignResponse>
    {
    public UserType PrimaryRole { get; set; } = UserType.Student;
    public List<UserType>? AdditionalRoles { get; set; }
    public string FullName { get; set; } = null!;
    public string TemporaryEmail { get; set; } = null!;
    public bool IsAcademicAdvisor { get; set; }
    public string? Phone { get; set; }
    public string? ImageName { get; set; }
    public string NationalNumber { get; set; } = null!;
    public GenderType? Gender { get; set; }
    public string? BirthDate { get; set; }
    public string FbToken { get; set; } = null!;
    public bool IsAndroidDevice { get; set; }
    public bool IsIosDevice { get; set; }
    public string? Password { get; set; }
    }

    public class GetUserProfileQuery : IRequest<GetUserProfileQueryResponse?>
    {
    }

    public class GetUserProfileQueryResponse
    {
    public Guid Id { get; set; }
    public string FullName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string? TemporaryEmail { get; set; }
    public bool IsUniversityEmailActivated { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Address { get; set; }
    public string? PhoneNumber { get; set; }
    public string NationalId { get; set; } = null!;
    public string Code { get; set; } = null!;
    }

    public class CareerSuggestionDto
    {
    public Guid Id { get; set; }
    public string CareerPath { get; set; } = null!;
    public string Justification { get; set; } = null!;
    public DateTime CreatedAt { get; set; }
    }

    public class GetCareerSuggestionsByStudentQuery : IRequest<PagginatedResult<CareerSuggestionDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public Guid StudentId { get; set; }
    }

    public class CreateNewCourseCommand : IRequest<string>
    {
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int CreditHours { get; set; }
    public Guid? DepartmentId { get; set; }
    }

    public class DeleteCourseCommand : IRequest<bool>
    {
    public Guid Id { get; set; }
    }

    public class DropCourseCommand : IRequest<bool>
    {
    public Guid CourseId { get; set; }
    }

    public class EnrollStudentCommand : IRequest<bool>
    {
    public Guid CourseId { get; set; }
    public Guid SectionId { get; set; }
    }

    public class UpdateCourseCommand : IRequest<string>
    {
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int CreditHours { get; set; }
    public Guid? DepartmentId { get; set; }
    }

    public class CourseDto
    {
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public int CreditHours { get; set; }
    }

    public class SectionDto
    {
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public Guid InstructorId { get; set; }
    public string Day { get; set; } = null!;
    public string Time { get; set; } = null!;
    public int Capacity { get; set; }
    }

    public class GetCourseDetailQuery : IRequest<CourseDetailDto>
    {
    public Guid CourseId { get; set; }
    public Guid? StudentId { get; set; }
    }

    public class AddDepartmentCommand : IRequest<Unit>
    {
    public string Name { get; set; } = null!;
    public string Code { get; set; } = null!;
    public string? Description { get; set; }
    }

    public class AssignCourseToDepartmentCommand : IRequest<string>
    {
    public Guid CourseId { get; set; }
    public Guid DepartmentId { get; set; }
    }

    public class AssignUserToDepartmentCommand : IRequest<string>
    {
    public Guid UserId { get; set; }
    public Guid DepartmentId { get; set; }
    }

    public class DeleteCourseFromDepartmentCommand : IRequest<string>
    {
    public Guid CourseId { get; set; }
    }

    public class DeleteDepartmentCommand: IRequest<string>
    {
    public Guid Id { get; set; }
    }

    public class DeleteUserFromDepartmentCommand : IRequest<ApiResponse<bool>>
    {
    public Guid UserId { get; set; }
    }

    public class UpdateDepartmentCommand: IRequest<Unit>
    {
    public Guid Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
    }

    public class CourseDto
    {
    public Guid Id { get; set; }
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public int CreditHours { get; set; }
    public string? Description { get; set; }
    }

    public class DepartmentDto
    {
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string? Code { get; set; }
    public string? Description { get; set; }
    }

    public class DepartmentUserDto
    {
    public Guid UserId { get; set; }
    public string FullName { get; set; } = null!;
    public string? Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string Role { get; set; } = null!; // Student, Doctor, Instructor
    public string? UniversityCode { get; set; }
    }

    public class GetAllCoursesInDepartmentPagginatedQuery : IRequest<PagginatedResult<CourseDto>>
    {
    public Guid DepartmentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class GetAllDepartmentsPagginatedQuery : IRequest<PagginatedResult<DepartmentDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? Search { get; set; }
    }

    public class GetAllUsersInDepartmentPagginatedQuery : IRequest<PagginatedResult<DepartmentUserDto>>
    {
    public Guid DepartmentId { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    }

    public class GetDepartmentByIdOrCodeQuery : IRequest<DepartmentDto>
    {
    public string IdOrCode { get; set; } = null!;
    }

    public class CreateDocumentRequestCommand : IRequest<Guid>
    {
    public Guid StudentId { get; set; }
    public string DocumentType { get; set; } = null!;
    public string? Remarks { get; set; }
    }

    public class DocumentRequestDto
    {
    public Guid Id { get; set; }
    public string DocumentType { get; set; } = null!;
    public string Status { get; set; } = null!; // Pending, Approved, Rejected, Completed
    public string? Remarks { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class AIRecommendationDto
    {
    public string Status { get; set; } = null!;
    public long StudentId { get; set; }
    public string DominantTrack { get; set; } = null!;
    public string TrackConfidence { get; set; } = null!;
    public string TrackReasoning { get; set; } = null!;
    }

    public class CourseRecommendationDto
    {
    public string Course { get; set; } = null!;
    public double Score { get; set; }
    public string Confidence { get; set; } = null!;
    }

    public class AITimetableRequestDto
    {
    public string Option { get; set; } = "balance";
    }

    public class TimetablePreferencesDto
    {
    public int BufferMinutes { get; set; } = 0;
    }

    public class CourseTimetableDataDto
    {
    public int Priority { get; set; }
    public int Difficulty { get; set; }
    public SessionDetailsDto Lecture { get; set; } = null!;
    }

    public class SessionDetailsDto
    {
    public string? Id { get; set; }
    public int Day { get; set; }
    public string StartTime { get; set; } = null!;
    public int Duration { get; set; }
    public int Capacity { get; set; }
    public int AvailableSeats { get; set; }
    }

    public class AITimetableResponseDto
    {
    public TimetableStatsDto Stats { get; set; } = null!;
    }

    public class TimetableStatsDto
    {
    public int MaxLoad { get; set; }
    public int TotalDays { get; set; }
    public string OptionName { get; set; } = null!;
    }

    public class ScheduledItemDto
    {
    public string Course { get; set; } = null!;
    public string Type { get; set; } = null!;
    public string? SectionId { get; set; }
    public int Day { get; set; }
    public string StartTime { get; set; } = null!;
    public string EndTime { get; set; } = null!;
    public int Difficulty { get; set; }
    public int Priority { get; set; }
    public int Capacity { get; set; }
    public int AvailableSeats { get; set; }
    }

    public class BatchStudentAIGradesDto
    {
    public string UniversityCode { get; set; } = null!;
    public float GPA { get; set; }
    }

    public class CourseMetadataDto
    {
    public string Code { get; set; } = null!;
    public string Title { get; set; } = null!;
    public string? Category { get; set; }
    }

    public class StudentAIGradesDto
    {
    public string StudentId { get; set; } = null!;
    public float GPA { get; set; }
    }

    public class GetAllStudentsGradesBatchQuery : IRequest<List<BatchStudentAIGradesDto>>
    {
    }

    public class GetCourseCatalogMetadataQuery : IRequest<List<CourseMetadataDto>>
    {
    }

    public class GetStudentGradesQuery : IRequest<StudentAIGradesDto>
    {
    public Guid StudentId { get; set; }
    }

    public class SimulateGPACommand : IRequest<float>
    {
    public Guid StudentId { get; set; }
    public float ExpectedGPA { get; set; }
    }

    public class GPASimulatorLogDto
    {
    public Guid Id { get; set; }
    public float ExpectedGPA { get; set; }
    public float PredictedCGPA { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class SubmitSectionGradesCommand : IRequest<Unit>
    {
    public Guid SectionId { get; set; }
    }

    public class GetInstructorDashboardQuery : IRequest<InstructorDashboardDto>
    {
    }

    public class GetStudentProfileForInstructorQuery : IRequest<InstructorStudentProfileDto>
    {
    public Guid StudentId { get; set; }
    }

    public class LearningResourceDto
    {
    public Guid Id { get; set; }
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public ResourceType ResourceType { get; set; }
    public string? ResourceUrl { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class CreateReminderCommand : IRequest<Unit>
    {
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    }

    public class DeleteReminderCommand : IRequest<bool>
    {
    public Guid Id { get; set; }
    }

    public class UpdateReminderCommand : IRequest<bool>
    {
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    }

    public class ReminderDto
    {
    public Guid Id { get; set; }
    public string Title { get; set; } = null!;
    public string? Description { get; set; }
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class GetRemindersPaginatedQuery : IRequest<PagginatedResult<ReminderDto>>
    {
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 20;
    public string? SearchTerm { get; set; }
    public bool? IsCompleted { get; set; }
    }

    public class BookAcademicServiceCommand : IRequest<Guid>
    {
    public AcademicServiceType ServiceType { get; set; }
    public DateTime AppointmentDate { get; set; }
    public TimeSpan Time { get; set; }
    public string Notes { get; set; } = null!;
    }

    public class SendRequestForAdvismentGuideCommand: IRequest<string>
    {
    public Guid StudentId { get; set; }
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    }

    public class ScheduleItemDto
    {
    public Guid CourseId { get; set; }
    public string CourseCode { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public Guid SectionId { get; set; }
    public string Day { get; set; } = null!;
    public string Time { get; set; } = null!;
    public string Status { get; set; } = null!;
    }

    public class StudentDashboardDto
    {
    public string FirstName { get; set; } = string.Empty;
    public float CGPA { get; set; }
    public int EarnedHours { get; set; }
    }

    public class TermGpaDto
    {
    public string TermName { get; set; } = string.Empty;
    public float Gpa { get; set; }
    }

    public class PlanProgressDto
    {
    public int CompletedCourses { get; set; }
    public int RemainingCourses { get; set; }
    public float UniversityRequirementsPercentage { get; set; }
    public float MajorRequirementsPercentage { get; set; }
    public float ElectiveRequirementsPercentage { get; set; }
    }

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

    public class StudentProfileDto
    {
    public string FullName { get; set; } = null!;
    public string UniversityCode { get; set; } = null!;
    public string? DepartmentName { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public float CurrentGPA { get; set; }
    public float CumulativeGPA { get; set; }
    public int Level { get; set; }
    public int CompletedHours { get; set; }
    public int TotalHours { get; set; } = 135; // Default as per UI design
    public string? AcademicAdvisorName { get; set; }
    }

    public class AcademicHistoryDto
    {
    public string SemesterName { get; set; } = null!;
    public float SemesterGPA { get; set; }
    }

    public class CourseGradeDto
    {
    public string CourseCode { get; set; } = null!;
    public string CourseTitle { get; set; } = null!;
    public int CreditHours { get; set; }
    }

    public class AcademicServiceInitialDataDto
    {
    public string StudentName { get; set; } = null!;
    public string UniversityCode { get; set; } = null!;
    public string DepartmentName { get; set; } = null!;
    public string AdvisorName { get; set; } = null!;
    public string AdvisorLocation { get; set; } = null!;
    }

    public class GetAcademicServiceInitialDataQuery : IRequest<AcademicServiceInitialDataDto>
    {
    }

    public class GetStudentDashboardQuery : IRequest<StudentDashboardDto>
    {
    public Guid UserId { get; set; }
    }

    public class GetStudentProfileQuery : IRequest<StudentProfileDto>
    {
    public Guid UserId { get; set; }
    }

    public class GetStudentScheduleQuery : IRequest<List<ScheduleItemDto>>
    {
    }

    public class StudyPlanDto
    {
    public Guid Id { get; set; }
    public Guid StudentId { get; set; }
    public Semester Semester { get; set; }
    public Guid CourseId { get; set; }
    public string? CourseName { get; set; }
    public DateTime CreatedAt { get; set; }
    }

    public class DownloadFileCommand: IRequest<string>
    {
    public int FilePlace { get; set; }
    public string FileName { get; set; } = null!;
    }

    public class UpdateImageCommand: IRequest<string>
    {
    public string ImageName { get; set; } = null!;
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UpdateVideoCommand : IRequest<string>
    {
    public string VideoName { get; set; } = null!;
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UploadAudioCommand: IRequest<string>
    {
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UploadFileCommand: IRequest<string>
    {
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UploadImageCommand : IRequest<string>
    {
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UploadMultipleImageCommand : IRequest<List<string>>
    {
    public int UploadPlace { get; set; }
    public List<IFormFile> Files { get; set; } = null!;
    }

    public class UploadVideoCommand: IRequest<string>
    {
    public int UploadPlace { get; set; }
    public IFormFile File { get; set; } = null!;
    }

    public class UploadMultipleVideoCommand: IRequest<List<string>>
    {
    public int UploadPlace { get; set; }
    public List<IFormFile> Files { get; set; } = null!;
    }

    public class HealthCheckResponse
    {
    public string OverallStatus { get; set; } = null!;
    public IEnumerable<HealthCheck> HealthChecks { get; set; } = null!;
    public string TotalDuration { get; set; } = null!;
    }

```

