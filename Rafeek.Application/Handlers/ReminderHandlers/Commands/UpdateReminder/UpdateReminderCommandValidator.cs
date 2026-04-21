using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.ReminderHandlers.Commands.UpdateReminder
{
    public class UpdateReminderCommandValidator : AbstractValidator<UpdateReminderCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateReminderCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Id)
                .NotEmpty();

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Reminder.TitleRequired.Value])
                .MaximumLength(200).WithMessage(_localizer[LocalizationKeys.Reminder.TitleMaxLength.Value]);

            RuleFor(x => x.DueDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.Reminder.DueDateRequired.Value]);
        }
    }
}
