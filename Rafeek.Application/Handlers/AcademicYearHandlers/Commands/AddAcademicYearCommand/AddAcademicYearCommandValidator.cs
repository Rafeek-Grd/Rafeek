using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands.AddAcademicYearCommand
{
    public class AddAcademicYearCommandValidator: AbstractValidator<AddAcademicYearCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public AddAcademicYearCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicYear.AcademicYearNameRequired]);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicYear.StartDateRequired]);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicYear.EndDateRequired]);

            RuleFor(x => x.EndDate)
                .GreaterThan(x => x.StartDate)
                .WithMessage(_localizer[LocalizationKeys.AcademicYear.EndDateMustBeGreaterThanStartDate]);
        }
    }
}
