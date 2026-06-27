using MediatR;
using Rafeek.Application.Handlers.InstructorHandlers.DTOs;

namespace Rafeek.Application.Handlers.InstructorHandlers.Queries.GetStudentProfileForInstructor
{
    public class GetStudentProfileForInstructorQuery : IRequest<InstructorStudentProfileDto>
    {
        public Guid StudentId { get; set; }
    }
}
