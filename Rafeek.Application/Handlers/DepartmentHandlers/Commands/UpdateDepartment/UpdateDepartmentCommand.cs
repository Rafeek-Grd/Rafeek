using MediatR;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.UpdateDepartment
{
    public class UpdateDepartmentCommand: IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
