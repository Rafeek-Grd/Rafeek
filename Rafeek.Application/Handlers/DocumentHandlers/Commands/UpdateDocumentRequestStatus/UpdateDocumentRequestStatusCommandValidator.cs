using FluentValidation;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.UpdateDocumentRequestStatus
{
    public class UpdateDocumentRequestStatusCommandValidator : AbstractValidator<UpdateDocumentRequestStatusCommand>
    {
        public UpdateDocumentRequestStatusCommandValidator()
        {
            RuleFor(x => x.RequestId)
                .NotEmpty();

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
