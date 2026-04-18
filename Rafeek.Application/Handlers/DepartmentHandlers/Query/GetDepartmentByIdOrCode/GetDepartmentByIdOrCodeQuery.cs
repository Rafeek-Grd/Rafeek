using MediatR;
using Rafeek.Application.Handlers.DepartmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Query.GetDepartmentByIdOrCode
{
    public class GetDepartmentByIdOrCodeQuery : IRequest<DepartmentDto>
    {
        public string IdOrCode { get; set; } = null!;
    }
}
