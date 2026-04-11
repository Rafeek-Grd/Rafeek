using MediatR;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAcademicTermById
{
    public class GetAcademicTermByIdQuery: IRequest<AcademicTermDto>
    {
        public Guid Id { get; set; }
    }
}
