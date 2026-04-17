using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands.CreateAcademicTerm
{
    public class CreateAcademicTermCommandValidator : AbstractValidator<CreateAcademicTermCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public CreateAcademicTermCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermNameRequired]);

            RuleFor(x => x.AcademicYearId)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermIdRequired]);

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermStartDateRequired]);

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.AcademicTerm.AcademicTermEndDateRequired])
                .GreaterThan(x => x.StartDate).WithMessage(_localizer[LocalizationKeys.AcademicTerm.EndDateMustBeGreaterThanStartDate]);

            RuleFor(x => x.RegistrationStartDate)
                .GreaterThan(x => x.StartDate)
                .When(x => x.RegistrationStartDate.HasValue)
                .WithMessage(_localizer[LocalizationKeys.AcademicTerm.RegistrationStartDateMustBeGreaterThanAcademicTermStartDate]);

            RuleFor(x => x.RegistrationEndDate)
                .GreaterThan(x => x.RegistrationStartDate)
                .When(x => x.RegistrationStartDate.HasValue && x.RegistrationEndDate.HasValue)
                .WithMessage(_localizer[LocalizationKeys.AcademicTerm.RegisterationEndDateMustBeGreaterThanRegistrationStartDate]);

            RuleFor(x => x.ExamStartDate)
                .GreaterThan(x => x.RegistrationStartDate)
                .When(x => x.ExamStartDate.HasValue && x.RegistrationStartDate.HasValue)
                .WithMessage(_localizer[LocalizationKeys.AcademicTerm.ExamStartDateMustBeGreaterThanRegistrationStartDate]);

            RuleFor(x => x.ExamEndDate)
                .GreaterThan(x => x.ExamStartDate)
                .When(x => x.ExamStartDate.HasValue && x.ExamEndDate.HasValue)
                .WithMessage(_localizer[LocalizationKeys.AcademicTerm.ExamEndDateMustBeGreaterThanExamStartDate]);
        }
    }
}
