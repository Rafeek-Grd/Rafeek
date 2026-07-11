using MediatR;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAITimetablesByStudent
{
    public class GetAITimetablesByStudentQuery : IRequest<List<AITimetableDto>>
    {
        public Guid StudentId { get; set; }
    }
}
