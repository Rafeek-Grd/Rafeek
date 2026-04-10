using FluentValidation;

namespace Rafeek.Application.Handlers.AcademicYearHandlers.Commands
{
    public class UpdateAcademicYearCommandValidator : AbstractValidator<UpdateAcademicYearCommand>
    {
        public UpdateAcademicYearCommandValidator()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("ID is required.");

            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Academic Year name is required.")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters.");

            RuleFor(x => x.StartDate)
                .NotEmpty().WithMessage("Start Date is required.");

            RuleFor(x => x.EndDate)
                .NotEmpty().WithMessage("End Date is required.")
                .GreaterThan(x => x.StartDate).WithMessage("End Date must be strictly after the Start Date.");
        }
    }
}
