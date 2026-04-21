using MediatR;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.DeleteReminder
{
    public class DeleteReminderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
    }
}
