using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.DeleteReminder
{
    public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand, bool>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReminderCommandHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Reminders
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _currentUserService.UserId, cancellationToken);

            if (entity == null)
                return false;

            _context.Reminders.Remove(entity);
            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
