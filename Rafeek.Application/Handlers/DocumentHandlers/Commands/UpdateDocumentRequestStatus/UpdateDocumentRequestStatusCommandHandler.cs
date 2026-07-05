using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.UpdateDocumentRequestStatus
{
    public class UpdateDocumentRequestStatusCommandHandler : IRequestHandler<UpdateDocumentRequestStatusCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public UpdateDocumentRequestStatusCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Handle(UpdateDocumentRequestStatusCommand request, CancellationToken cancellationToken)
        {
            var entity = await _ctx.DocumentRequestRepository
                .GetFirstIncludingAll(x => x.Id == request.RequestId)
                .FirstOrDefaultAsync(cancellationToken);

            if (entity == null)
            {
                throw new NotFoundException(nameof(DocumentRequest), request.RequestId);
            }

            entity.Status = request.Status;
            entity.Remarks = request.Remarks;

            await _ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
