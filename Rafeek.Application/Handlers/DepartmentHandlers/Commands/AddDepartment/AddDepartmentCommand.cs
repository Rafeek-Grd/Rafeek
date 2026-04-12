using MediatR;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AddDepartment
{
    public class AddDepartmentCommand : IRequest<Unit>
    {
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string? Description { get; set; }
    }
}
