using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.DeleteDocumentRequest
{
    public class DeleteDocumentRequestCommandHandler : IRequestHandler<DeleteDocumentRequestCommand, string>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;

        public DeleteDocumentRequestCommandHandler(IUnitOfWork ctx, IStringLocalizer<Messages> localizer)
        {
            _ctx = ctx;
            _localizer = localizer;
        }

        public async Task<string> Handle(DeleteDocumentRequestCommand request, CancellationToken cancellationToken)
        {
            if (request.DocumentRequestId.HasValue)
            {
                var entity = await _ctx.DocumentRequestRepository.FindByKeyAsync(request.DocumentRequestId.Value, cancellationToken);

                entity!.IsActive = false;
                entity.IsDeleted = true;
                _ctx.DocumentRequestRepository.Update(entity);
            }
            else
            {
                var entities = await _ctx.DocumentRequestRepository.GetAll().ToListAsync(cancellationToken);

                if(!entities.Any())
                    return _localizer[LocalizationKeys.GlobalValidationMessages.NoRecordsFoundToDelete.Value];

                entities.ForEach(entity =>
                {
                    entity.IsActive = false;
                    entity.IsDeleted = true;
                });

                _ctx.DocumentRequestRepository.UpdateRange(entities);   
            }

            var result = await _ctx.SaveChangesAsync(cancellationToken);
            return result > 0
                   ? _localizer[LocalizationKeys.GlobalValidationMessages.DeletedSuccessfully.Value]
                   : _localizer[LocalizationKeys.GlobalValidationMessages.DeletedFailed.Value];
        }
    }
}
