using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AIHandlers.Commands.SaveAITimetable
{
    public class SaveAITimetableCommandValidator : AbstractValidator<SaveAITimetableCommand>
    {
        public SaveAITimetableCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.StudentId)
                .NotEmpty().WithMessage(localizer["StudentId_Required"]);

            RuleFor(v => v.TimetableData)
                .NotNull().WithMessage(localizer["TimetableData_Required"]);

            RuleFor(v => v.TimetableData.Stats)
                .NotNull().When(v => v.TimetableData != null);
            
            RuleFor(v => v.TimetableData.Schedule)
                .NotEmpty().When(v => v.TimetableData != null);
        }
    }
}
