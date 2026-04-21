using MediatR;
using System;

namespace Rafeek.Application.Handlers.CourseHandlers.Commands.EnrollStudent
{
    public class EnrollStudentCommand : IRequest<bool>
    {
        public Guid CourseId { get; set; }
        public Guid SectionId { get; set; }
    }
}
