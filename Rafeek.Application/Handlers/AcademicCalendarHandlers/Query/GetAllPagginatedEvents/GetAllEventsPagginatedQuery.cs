using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicCalendarHandlers.DTOs;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Query.GetAllPagginatedEvents
{
    public class GetAllEventsPagginatedQuery: IRequest<PagginatedResult<AcademicCalendarDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
