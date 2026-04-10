using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.DeleteEventOfAcademicCalendar
{
    public class DeleteEventOfAcademicCalendarCommandValidator: AbstractValidator<DeleteEventOfAcademicCalendarCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteEventOfAcademicCalendarCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;
        }

        public DeleteEventOfAcademicCalendarCommandValidator()
        {
            RuleFor(x => x.AcademicEventId)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventIdRequired.Value]);

            RuleFor(x => x.AcademicEventId)
                .Must(id => Guid.TryParse(id, out _))
                .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventIdNotValid.Value]);
        }
    }
}
