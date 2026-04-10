using FluentValidation;

namespace Rafeek.Application.Handlers.AcademicTermHandlers.Commands
{
    public class UpdateAcademicTermCommandValidator : AbstractValidator<UpdateAcademicTermCommand>
    {
        public UpdateAcademicTermCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Name is required.");

            RuleFor(x => x.AcademicYearId)
                .NotEmpty().WithMessage("Academic Year ID is required.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End Date must be after Start Date.");

            RuleFor(x => x.RegistrationEndDate)
                .GreaterThan(x => x.RegistrationStartDate)
                .When(x => x.RegistrationStartDate.HasValue && x.RegistrationEndDate.HasValue)
                .WithMessage("Registration End Date must be after Registration Start Date.");
        }
    }
}
