using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicSchedules.DTOs;

namespace Rafeek.Application.Handlers.AcademicSchedules.Queries.GetAcademicSchedules
{
    public class GetAcademicSchedulesQuery: IRequest<PagginatedResult<AcademicScheduleDto>>
    {
        public Guid? TermId { get; set; }
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }
}
