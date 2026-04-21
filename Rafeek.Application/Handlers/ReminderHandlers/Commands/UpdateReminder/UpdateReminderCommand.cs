using MediatR;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.UpdateReminder
{
    public class UpdateReminderCommand : IRequest<bool>
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
        public bool IsCompleted { get; set; }
    }
}
