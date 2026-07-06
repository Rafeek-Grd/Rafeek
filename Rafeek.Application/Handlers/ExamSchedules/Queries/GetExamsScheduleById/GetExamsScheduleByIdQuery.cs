using MediatR;
using Rafeek.Application.Handlers.ExamSchedules.DTOs;

namespace Rafeek.Application.Handlers.ExamSchedules.Queries.GetExamsScheduleById
{
    public class GetExamsScheduleByIdQuery : IRequest<ExamItemDto>
    {
        public Guid Id { get; set; }
    }
}