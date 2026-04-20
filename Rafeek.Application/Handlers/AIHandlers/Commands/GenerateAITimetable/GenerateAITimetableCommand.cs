using MediatR;
using Rafeek.Application.Common.Models.AI;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable
{
    public class GenerateAITimetableCommand : IRequest<AITimetableResponseDto>
    {
        public AITimetableRequestDto TimetableRequest { get; set; } = null!;
    }
}
