using MediatR;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetEventOfAcademicCalendarById
{
    public class GetEventOfAcademicCalendarByIdQuery: IRequest<AcademicCalendarDto>
    {
        public Guid AcademicCalendarId { get; set; }
    }
}
