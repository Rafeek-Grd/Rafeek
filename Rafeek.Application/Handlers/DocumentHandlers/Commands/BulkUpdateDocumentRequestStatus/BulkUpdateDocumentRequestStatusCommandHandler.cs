using MediatR;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.BulkUpdateDocumentRequestStatus
{
    public class BulkUpdateDocumentRequestStatusCommandHandler : IRequestHandler<BulkUpdateDocumentRequestStatusCommand, bool>
    {
        private readonly IUnitOfWork _ctx;

        public BulkUpdateDocumentRequestStatusCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<bool> Handle(BulkUpdateDocumentRequestStatusCommand request, CancellationToken cancellationToken)
        {
            if (request.RequestIds == null || !request.RequestIds.Any())
            {
                return false;
            }

            var entities = await _ctx.DocumentRequestRepository
                .IncludeAll(x => request.RequestIds.Contains(x.Id))
                .ToListAsync(cancellationToken);

            foreach (var entity in entities)
            {
                entity.Status = request.Status;
                if (!string.IsNullOrWhiteSpace(request.Remarks))
                {
                    entity.Remarks = request.Remarks;
                }
            }

            await _ctx.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
