using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicSchedules.Commands.CreateAcadmicSchedule
{
    public class CreateAcadmicScheduleCommandValidator : AbstractValidator<CreateAcadmicScheduleCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateAcadmicScheduleCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.CourseId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermIdRequired.Value]);

            RuleFor(x => x.Day)
                .NotEmpty().WithMessage(_localizer["DayRequired"]);

            RuleFor(x => x.Time)
                .NotEmpty().WithMessage(_localizer["TimeRequired"]);

            RuleFor(x => x.Capacity)
                .GreaterThanOrEqualTo(0).WithMessage(_localizer["CapacityInvalid"]);
        }
    }
}
