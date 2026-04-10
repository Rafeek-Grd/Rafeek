using FluentValidation;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Commands
{
    public class ReviewStudentGuidanceRequestCommandValidator : AbstractValidator<ReviewStudentGuidanceRequestCommand>
    {
        public ReviewStudentGuidanceRequestCommandValidator()
        {
            RuleFor(x => x.AdvisorId)
                .NotEmpty().WithMessage("Advisor ID is required.");

            RuleFor(x => x.RequestId)
                .NotEmpty().WithMessage("Request ID is required.");

            RuleFor(x => x.Status)
                .IsInEnum().WithMessage("Invalid status value.");
        }
    }
}
