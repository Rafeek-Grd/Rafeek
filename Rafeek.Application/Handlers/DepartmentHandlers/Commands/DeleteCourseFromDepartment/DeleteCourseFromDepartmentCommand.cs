using MediatR;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Handlers.DepartmentHandlers.Commands.DeleteCourseFromDepartment
{
    public class DeleteCourseFromDepartmentCommand : IRequest<string>
    {
        public Guid CourseId { get; set; }
    }
}
