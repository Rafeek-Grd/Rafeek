using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignUserToDepartment
{
    public class AssignUserToDepartmentCommand : IRequest<string>
    {
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
