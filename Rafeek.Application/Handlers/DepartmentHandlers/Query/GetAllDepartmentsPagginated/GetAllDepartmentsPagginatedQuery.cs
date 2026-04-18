using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllDepartmentsPagginated
{
    public class GetAllDepartmentsPagginatedQuery : IRequest<PagginatedResult<DepartmentDto>>
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public string? Search { get; set; }
    }
}
