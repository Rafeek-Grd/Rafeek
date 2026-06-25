using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentSchedule
{
    public class GetStudentScheduleQuery : IRequest<PagginatedResult<ScheduleItemDto>>
    {
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }
}
