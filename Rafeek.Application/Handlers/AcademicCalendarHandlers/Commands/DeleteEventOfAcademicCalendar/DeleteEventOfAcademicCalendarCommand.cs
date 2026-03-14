using MediatR;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.DeleteEventOfAcademicCalendar
{
    public class DeleteEventOfAcademicCalendarCommand: IRequest<string>
    {
        public string AcademicEventId { get; set; } = string.Empty;
    }
}
