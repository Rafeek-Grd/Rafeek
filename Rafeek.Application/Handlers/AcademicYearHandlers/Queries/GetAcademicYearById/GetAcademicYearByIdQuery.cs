using MediatR;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAcademicYearById
{
    public class GetAcademicYearByIdQuery : IRequest<AcademicYearDto?>
    {
        public Guid Id { get; set; }
    }
}
