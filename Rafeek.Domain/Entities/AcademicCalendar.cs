using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class AcademicCalendar : BaseEntity
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

        // Recurrence
        public RecurrenceType RecurrenceType { get; set; } = RecurrenceType.None;
        public DateTime? RecurrenceEndDate { get; set; }

        // Relationships
        public Guid? TargetUserId { get; set; }
        public ApplicationUser? TargetUser { get; set; }

        public Guid? AcademicTermId { get; set; }
        public AcademicTerm? AcademicTerm { get; set; }

        public Guid? DepartmentId { get; set; }
        public Department? Department { get; set; }

        public Guid? CourseId { get; set; }
        public Course? Course { get; set; }

        public Guid? SectionId { get; set; }
        public Section? Section { get; set; }
    }
}
