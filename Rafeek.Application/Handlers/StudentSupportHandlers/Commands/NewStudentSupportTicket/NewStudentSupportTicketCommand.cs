using MediatR;
using Rafeek.Application.Handlers.StudentSupportHandlers.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.NewStudentSupportTicket
{
    public class NewStudentSupportTicketCommand : IRequest<NewStudentSupportDto>
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public StudentSupportType TicketType { get; set; }
    }
}
