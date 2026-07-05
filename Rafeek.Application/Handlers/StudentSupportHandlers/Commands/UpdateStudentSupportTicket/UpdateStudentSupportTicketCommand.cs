using MediatR;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.UpdateStudentSupportTicket
{
    public class UpdateStudentSupportTicketCommand : IRequest<string>
    {
        public Guid Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
    }
}
