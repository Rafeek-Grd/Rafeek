using MediatR;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands
{
    public class AddEventToAcademicCalendarCommand: IRequest<string>
    {
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string? Location { get; set; }

        public AcademicCalendarEventType EventType { get; set; } = AcademicCalendarEventType.General;
        public Guid? TargetUserId { get; set; }
    }
}
