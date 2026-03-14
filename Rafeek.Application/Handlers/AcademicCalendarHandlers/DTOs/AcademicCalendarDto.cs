using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs
{
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
}
