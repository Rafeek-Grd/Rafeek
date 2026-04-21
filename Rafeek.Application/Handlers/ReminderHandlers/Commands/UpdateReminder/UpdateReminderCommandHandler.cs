using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.UpdateReminder
{
    public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, bool>
    {
        private readonly IRafeekDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public UpdateReminderCommandHandler(IRafeekDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Reminders
                .FirstOrDefaultAsync(x => x.Id == request.Id && x.UserId == _currentUserService.UserId, cancellationToken);

            if (entity == null)
                return false;

            entity.Title = request.Title;
            entity.Description = request.Description;
            entity.DueDate = request.DueDate;
            entity.IsCompleted = request.IsCompleted;

            await _context.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
