using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.DeleteReminder
{
    public class DeleteReminderCommandHandler : IRequestHandler<DeleteReminderCommand, bool>
    {
        private readonly IUnitOfWork _ctx;
        private readonly ICurrentUserService _currentUserService;

        public DeleteReminderCommandHandler(IUnitOfWork ctx, ICurrentUserService currentUserService)
        {
            _ctx = ctx;
            _currentUserService = currentUserService;
        }

        public async Task<bool> Handle(DeleteReminderCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.ReminderRepository
                .GetBy(x => x.Id == request.Id && x.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
                return false;

            _ctx.ReminderRepository.Delete(entity);
            await _ctx.SaveChangesAsync(cancellationToken);
            return true;
        }
    }
}
