using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.GenerateAITimetable
{
    public class GenerateAITimetableCommandValidator : AbstractValidator<GenerateAITimetableCommand>
    {
        public GenerateAITimetableCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.TimetableRequest)
                .NotNull().WithMessage(localizer["TimetableRequest_Required"]);

            RuleFor(v => v.TimetableRequest.Courses)
                .NotEmpty().WithMessage(localizer["Courses_Required"]);

            RuleForEach(v => v.TimetableRequest.Courses)
                .ChildRules(course => {
                    course.RuleFor(c => c.Value.Lecture)
                        .Must(l => l != null && l.AvailableSeats > 0)
                        .WithMessage((c, _) => string.Format(localizer["Lecture_Full"], c.Key));
                });
        }
    }
}
