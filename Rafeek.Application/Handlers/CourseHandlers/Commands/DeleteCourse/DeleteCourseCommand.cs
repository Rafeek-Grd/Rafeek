using MediatR;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DeleteCourse
{
    public class DeleteCourseCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
