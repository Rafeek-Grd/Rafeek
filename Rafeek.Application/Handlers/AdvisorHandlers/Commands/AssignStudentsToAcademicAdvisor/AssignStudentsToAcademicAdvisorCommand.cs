using MediatR;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands.AssignStudentsToAcademicAdvisor
{
    public class AssignStudentsToAcademicAdvisorCommand: IRequest<Unit>
    {
        public Guid AcademicAdvisorId { get; set; }
        public List<Guid> StudentIds { get; set; } = null!;
    }
}
