using MediatR;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.StudentHandlers.Query.GetStudentSchedule
{
    public class GetStudentScheduleQuery : IRequest<List<ScheduleItemDto>>
    {
    }
}
