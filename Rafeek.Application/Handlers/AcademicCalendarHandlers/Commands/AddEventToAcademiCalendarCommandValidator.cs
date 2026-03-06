using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands
{
    public class AddEventToAcademiCalendarCommandValidator: AbstractValidator<AddEventToAcademicCalendarCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public AddEventToAcademiCalendarCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.EventName)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventNameRequired.Value])
                .MaximumLength(200).WithMessage(_localizer[LocalizationKeys.AcademicCalendar.MaxLengthOfEventNameExceededRequiredLength.Value]);

            RuleFor(x => x.EventDate).NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventDateRequired.Value]);

            When(x => !x.IsAllDay, () =>
            {
                RuleFor(x => x.StartTime).NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.StartTimeRequired.Value]);
                RuleFor(x => x.EndTime).NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeRequired.Value]);
                RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeMustBeGreaterThanStartTime.Value]);
            });

            RuleFor(x => x.Location)
                .MaximumLength(200)
                .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.MaxLengthOfLocationExceededRequiredLength.Value])
                .When(x => !string.IsNullOrEmpty(x.Location));

            When(x => x.EventType == Rafeek.Domain.Entities.AcademicCalendarEventType.Guidance, () =>
            {
                RuleFor(x => x.TargetUserId)
                .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.TargetUserIdRequired.Value]);
            });
        }
    }
}
