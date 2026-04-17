using MediatR;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.DeleteAcademicYear
{
    public class DeleteAcademicYearCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
    }
}
