using MediatR;

namespace Rafeek.Application.Handlers.CourseSectionHandlers.Commands.DeleteCourseSection
{
    public class DeleteCourseSectionCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
