using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteUserFromDepartment
{
    public class DeleteUserFromDepartmentCommand : IRequest<ApiResponse<bool>>
    {
        public Guid UserId { get; set; }
    }
}
