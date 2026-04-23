using MediatR;
using Rafeek.Application.Handlers.ExternalHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable
{
    public class GenerateAITimetableCommand : IRequest<AITimetableResponseDto>
    {
        public AITimetableRequestDto TimetableRequest { get; set; } = null!;
    }
}
