using MediatR;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAllAcademicYear
{
    public class GetAllAcademicYearsQuery : IRequest<List<AcademicYearDto>>
    {
    }
}
