using MediatR;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.DeleteAcademicTerm
{
    public class DeleteAcademicTermCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
