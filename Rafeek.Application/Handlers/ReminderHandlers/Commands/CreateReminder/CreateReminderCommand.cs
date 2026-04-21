using MediatR;
using System;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.CreateReminder
{
    public class CreateReminderCommand : IRequest<Unit>
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public DateTime DueDate { get; set; }
    }
}
