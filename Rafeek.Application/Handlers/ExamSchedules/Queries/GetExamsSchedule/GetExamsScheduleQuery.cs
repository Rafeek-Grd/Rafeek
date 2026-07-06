using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ExamSchedules.DTOs;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsSchedule
{
    public class GetExamsScheduleQuery: IRequest<PagginatedResult<ExamDayGroupDto>>
    {
        public Guid? TermId { get; set; }
        public string? SearchText { get; set; }
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }
}
