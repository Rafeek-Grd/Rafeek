using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicYearHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Queries.GetAllAcademicYear
{
    public class GetAllAcademicYearsPagginatedQuery : IRequest<PagginatedResult<AcademicYearDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
