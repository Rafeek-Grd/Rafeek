using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.DeleteReminder
{
    public class DeleteReminderCommandValidator: AbstractValidator<DeleteReminderCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteReminderCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            RuleFor(x => x.Id)
                .MustAsync(ReminderExists)
                .WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
        }

        private async Task<bool> ReminderExists(Guid reminderId, CancellationToken cancellationToken)
        {
            return await _ctx.ReminderRepository.ExistsAsync(r => r.Id == reminderId, cancellationToken);
        }
    }
}
