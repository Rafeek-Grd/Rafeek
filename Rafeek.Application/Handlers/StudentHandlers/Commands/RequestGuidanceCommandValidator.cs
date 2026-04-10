using FluentValidation;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands
{
    public class RequestGuidanceCommandValidator : AbstractValidator<RequestGuidanceCommand>
    {
        public RequestGuidanceCommandValidator()
        {
            RuleFor(x => x.StudentId)
                .NotEmpty().WithMessage("Student ID is required.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(150).WithMessage("Title cannot exceed 150 characters.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
        }
    }
}
