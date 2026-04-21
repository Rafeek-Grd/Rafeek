using MediatR;
using System;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.DropCourse
{
    public class DropCourseCommand : IRequest<bool>
    {
        public Guid CourseId { get; set; }
    }
}
