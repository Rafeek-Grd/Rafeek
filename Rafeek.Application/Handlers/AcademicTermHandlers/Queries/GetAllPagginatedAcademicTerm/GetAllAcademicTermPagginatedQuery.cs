using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AcademicTermHandlers.DTOs;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Queries.GetAllPagginatedAcademicTerm
{
    public class GetAllAcademicTermPagginatedQuery: IRequest<PagginatedResult<AcademicTermDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
