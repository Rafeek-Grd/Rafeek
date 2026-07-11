using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable
{
    public class GenerateAITimetableCommandValidator : AbstractValidator<GenerateAITimetableCommand>
    {
        private static readonly string[] ValidOptions = ["balance", "early", "late"];

        public GenerateAITimetableCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.TimetableRequest)
                .NotNull().WithMessage(localizer[LocalizationKeys.AI.TimetableRequestRequired.Value]);

            RuleFor(v => v.TimetableRequest.Option)
                .Must(o => ValidOptions.Contains(o))
                .WithMessage(localizer[LocalizationKeys.AI.OptionInvalid.Value])
                .When(v => v.TimetableRequest != null);

            RuleFor(v => v.TimetableRequest.Preferences)
                .NotNull().WithMessage(localizer[LocalizationKeys.AI.PreferencesRequired.Value])
                .ChildRules(prefs =>
                {
                    prefs.RuleFor(p => p.BufferMinutes)
                        .GreaterThanOrEqualTo(0).WithMessage(localizer[LocalizationKeys.AI.BufferMinutesInvalid.Value]);
                }).When(v => v.TimetableRequest != null);

            RuleFor(v => v.TimetableRequest.Courses)
                .NotEmpty().WithMessage(localizer[LocalizationKeys.AI.CoursesRequired.Value]);

            RuleForEach(v => v.TimetableRequest.Courses)
                .ChildRules(course =>
                {
                    course.RuleFor(c => c.Key)
                        .NotEmpty().WithMessage(localizer[LocalizationKeys.AI.CourseNameRequired.Value]);

                    course.RuleFor(c => c.Value)
                        .NotNull().WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.CourseDataRequired.Value], c.Key));

                    course.RuleFor(c => c.Value.Priority)
                        .InclusiveBetween(1, 5).WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.PriorityInvalid.Value], c.Key));

                    course.RuleFor(c => c.Value.Difficulty)
                        .InclusiveBetween(1, 5).WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.DifficultyInvalid.Value], c.Key));

                    course.RuleFor(c => c.Value.Lecture)
                        .NotNull().WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.LectureRequired.Value], c.Key))
                        .Must(l => l.AvailableSeats > 0).WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.LectureFull.Value], c.Key))
                        .ChildRules(lecture =>
                        {
                            lecture.RuleFor(l => l.Day)
                                .InclusiveBetween(0, 6).WithMessage(localizer[LocalizationKeys.AI.DayInvalid.Value]);

                            lecture.RuleFor(l => l.Duration)
                                .GreaterThan(0).WithMessage(localizer[LocalizationKeys.AI.DurationInvalid.Value]);

                            lecture.RuleFor(l => l.Capacity)
                                .GreaterThan(0).WithMessage(localizer[LocalizationKeys.AI.CapacityInvalid.Value]);
                        });

                    course.RuleFor(c => c.Value.Sections)
                        .Must(sections => sections.Any(s => s.AvailableSeats > 0))
                        .WithMessage((c, _) => string.Format(localizer[LocalizationKeys.AI.SectionsFull.Value], c.Key));

                    course.RuleForEach(c => c.Value.Sections)
                        .ChildRules(section =>
                        {
                            section.RuleFor(s => s.Day)
                                .InclusiveBetween(0, 6).WithMessage(localizer[LocalizationKeys.AI.DayInvalid.Value]);

                            section.RuleFor(s => s.Duration)
                                .GreaterThan(0).WithMessage(localizer[LocalizationKeys.AI.DurationInvalid.Value]);

                            section.RuleFor(s => s.Capacity)
                                .GreaterThan(0).WithMessage(localizer[LocalizationKeys.AI.CapacityInvalid.Value]);

                            section.RuleFor(s => s.AvailableSeats)
                                .GreaterThanOrEqualTo(0).WithMessage(localizer[LocalizationKeys.AI.AvailableSeatsInvalid.Value]);
                        });
                });
        }
    }
}
