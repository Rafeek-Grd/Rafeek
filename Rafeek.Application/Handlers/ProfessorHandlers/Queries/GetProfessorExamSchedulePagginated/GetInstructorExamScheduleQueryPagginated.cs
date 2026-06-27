using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetInstructorExamSchedule
{
    public class GetInstructorExamScheduleQueryPagginated : IRequest<PagginatedResult<InstructorExamScheduleDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
