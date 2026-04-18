using MediatR;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.AssignCourseToDepartment
{
    public class AssignCourseToDepartmentCommand : IRequest<string>
    {
        public Guid CourseId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
