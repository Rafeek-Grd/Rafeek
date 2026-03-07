using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

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

            When(x => x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.EndDate)
                    .GreaterThanOrEqualTo(x => x.EventDate)
                    .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndDateMustBeGreaterThanOrEqualEventDate.Value]);
            });

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

            When(x => x.RecurrenceType != RecurrenceType.None, () =>
            {
                RuleFor(x => x.RecurrenceEndDate)
                    .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.RecurrenceEndDateRequired.Value]);

                RuleFor(x => x.RecurrenceEndDate)
                    .GreaterThan(x => x.EventDate)
                    .When(x => x.RecurrenceEndDate.HasValue)
                    .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.RecurrenceEndDateMustBeAfterEventDate.Value]);
            });

            When(x => x.EventType == AcademicCalendarEventType.Guidance, () =>
            {
                RuleFor(x => x.TargetUserId)
                .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.TargetUserIdRequired.Value]);
            });

            When(x => x.EventType == AcademicCalendarEventType.Exam, () =>
            {
                RuleFor(x => x.CourseId)
                    .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.CourseIdRequiredForExamEvent.Value]);
            });
        }
    }
}
