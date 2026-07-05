using MediatR;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Queries.GetStudentSupportById
{
    public class GetStudentSupportByIdQuery : IRequest<NewStudentSupportDto>
    {
        public Guid Id { get; set; }
    }
}
