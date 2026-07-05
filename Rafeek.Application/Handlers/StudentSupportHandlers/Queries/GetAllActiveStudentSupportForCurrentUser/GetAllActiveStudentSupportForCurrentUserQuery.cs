using MediatR;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetAllActiveStudentSupportForCurrentUser
{
    public class GetAllActiveStudentSupportForCurrentUserQuery : IRequest<List<NewStudentSupportDto>>
    {
        public string? Email { get; set; }
    }
}
