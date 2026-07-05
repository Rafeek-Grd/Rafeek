using MediatR;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.Commands.DeleteStudentSupportTicket
{
    public class DeleteStudentSupportTicketCommand: IRequest<string>
    {
        public Guid? MessageSupportId { get; set; }
    }
}
