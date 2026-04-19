using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllDepartmentsPagginated
{
    public class GetAllDepartmentsPagginatedQuery : IRequest<PagginatedResult<DepartmentDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? Search { get; set; }
    }
}
