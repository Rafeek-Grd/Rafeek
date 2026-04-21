using MediatR;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.UpdateCourse
{
    public class UpdateCourseCommand : IRequest<string>
    {
        public Guid Id { get; set; }
        public string Code { get; set; } = null!;
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public int CreditHours { get; set; }
        public Guid? DepartmentId { get; set; }
    }
}
