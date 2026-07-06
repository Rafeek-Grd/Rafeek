using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.DeleteDocumentRequest
{
    public class DeleteDocumentRequestCommandValidator : AbstractValidator<DeleteDocumentRequestCommand>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteDocumentRequestCommandValidator(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;

            When(_ => _.DocumentRequestId.HasValue, () =>
            {
                RuleFor(_ => _.DocumentRequestId)
                    .MustAsync(DocumentRequestExists)
                    .WithMessage(_localizer[LocalizationKeys.ExceptionMessage.NotFound.Value]);
            });
        }

        private async Task<bool> DocumentRequestExists(Guid? documentRequestId, CancellationToken cancellationToken)
        {
            return await _ctx.DocumentRequestRepository.ExistsAsync(dr => dr.Id == documentRequestId, cancellationToken);
        }
    }
}
