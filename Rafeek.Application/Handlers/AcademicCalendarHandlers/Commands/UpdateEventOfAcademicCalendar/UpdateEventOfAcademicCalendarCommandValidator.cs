using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AcademicCalendarHandlers.Commands.UpdateEventOfAcademicCalendar
{
    public class UpdateEventOfAcademicCalendarCommandValidator : AbstractValidator<UpdateEventOfAcademicCalendarCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateEventOfAcademicCalendarCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Id).NotEmpty();

            // For PATCH allow nullable properties; validate only when provided
            When(x => x.EventName != null, () =>
            {
                RuleFor(x => x.EventName)
                    .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventNameRequired.Value])
                    .MaximumLength(200).WithMessage(_localizer[LocalizationKeys.AcademicCalendar.MaxLengthOfEventNameExceededRequiredLength.Value]);
            });

            When(x => x.EventDate.HasValue, () =>
            {
                RuleFor(x => x.EventDate).NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EventDateRequired.Value]);
            });

            When(x => x.EndDate.HasValue, () =>
            {
                RuleFor(x => x.EndDate)
                    .GreaterThanOrEqualTo(x => x.EventDate ?? DateTime.MinValue)
                    .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndDateMustBeGreaterThanOrEqualEventDate.Value]);
            });

            When(x => x.IsAllDay.HasValue && !x.IsAllDay.Value, () =>
            {
                When(x => x.StartTime.HasValue, () =>
                {
                    RuleFor(x => x.StartTime).NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.StartTimeRequired.Value]);
                });

                When(x => x.EndTime.HasValue, () =>
                {
                    RuleFor(x => x.EndTime).NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeRequired.Value]);
                });

                // Only validate ordering if both provided
                When(x => x.StartTime.HasValue && x.EndTime.HasValue, () =>
                {
                    RuleFor(x => x.EndTime).GreaterThan(x => x.StartTime).WithMessage(_localizer[LocalizationKeys.AcademicCalendar.EndTimeMustBeGreaterThanStartTime.Value]);
                });
            });

            When(x => x.Location != null, () =>
            {
                RuleFor(x => x.Location)
                    .MaximumLength(200)
                    .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.MaxLengthOfLocationExceededRequiredLength.Value]);
            });

            When(x => x.RecurrenceType.HasValue && x.RecurrenceType.Value != RecurrenceType.None, () =>
            {
                RuleFor(x => x.RecurrenceEndDate)
                    .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.RecurrenceEndDateRequired.Value]);

                When(x => x.RecurrenceEndDate.HasValue && x.EventDate.HasValue, () =>
                {
                    RuleFor(x => x.RecurrenceEndDate)
                        .GreaterThan(x => x.EventDate.Value)
                        .WithMessage(_localizer[LocalizationKeys.AcademicCalendar.RecurrenceEndDateMustBeAfterEventDate.Value]);
                });
            });

            When(x => x.EventType.HasValue && x.EventType.Value == AcademicCalendarEventType.Guidance, () =>
            {
                RuleFor(x => x.TargetUserId)
                .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.TargetUserIdRequired.Value]);
            });

            When(x => x.EventType.HasValue && x.EventType.Value == AcademicCalendarEventType.Exam, () =>
            {
                RuleFor(x => x.CourseId)
                    .NotNull().WithMessage(_localizer[LocalizationKeys.AcademicCalendar.CourseIdRequiredForExamEvent.Value]);
            });
        }
    }
}
