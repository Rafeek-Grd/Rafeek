using FluentValidation;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.BulkUpdateDocumentRequestStatus
{
    public class BulkUpdateDocumentRequestStatusCommandValidator : AbstractValidator<BulkUpdateDocumentRequestStatusCommand>
    {
        public BulkUpdateDocumentRequestStatusCommandValidator()
        {
            RuleFor(x => x.RequestIds)
                .NotEmpty();

            RuleFor(x => x.Status)
                .IsInEnum();
        }
    }
}
