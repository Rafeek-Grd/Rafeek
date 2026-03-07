using MediatR;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands
{
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
}
