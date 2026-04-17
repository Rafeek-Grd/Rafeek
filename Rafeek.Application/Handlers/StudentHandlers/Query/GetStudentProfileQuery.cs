using MediatR;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Query
{
    public class GetStudentProfileQuery : IRequest<StudentProfileDto>
    {
        public Guid UserId { get; set; }
    }
}
