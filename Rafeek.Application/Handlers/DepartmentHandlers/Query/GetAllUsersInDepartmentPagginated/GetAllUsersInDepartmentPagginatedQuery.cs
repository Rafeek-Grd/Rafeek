using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetAllUsersInDepartmentPagginated
{
    public class GetAllUsersInDepartmentPagginatedQuery : IRequest<PagginatedResult<DepartmentUserDto>>
    {
        public Guid DepartmentId { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
