using MediatR;
using Rafeek.Application.Common.Models.AI;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.SaveAITimetable
{
    public class SaveAITimetableCommand : IRequest<Guid>
    {
        public Guid? Id { get; set; }
        public Guid StudentId { get; set; }
        public string? TimetableName { get; set; }
        public AITimetableResponseDto TimetableData { get; set; } = null!;
    }
}
