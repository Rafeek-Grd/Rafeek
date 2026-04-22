using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Commands.CreateAssignment
{
    public class CreateAssignmentCommand : IRequest<Guid>
    {
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
        public DateTime DueDate { get; set; }
        public float TotalScore { get; set; }
        public Guid SectionId { get; set; }
    }
}
