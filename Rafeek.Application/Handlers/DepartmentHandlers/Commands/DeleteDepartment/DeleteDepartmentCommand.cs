using MediatR;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteDepartment
{
    public class DeleteDepartmentCommand: IRequest<string>
    {
        public Guid Id { get; set; }
    }
}
