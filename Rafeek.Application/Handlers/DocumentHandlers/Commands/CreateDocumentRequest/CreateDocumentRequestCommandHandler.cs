using MediatR;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.CreateDocumentRequest
{
    public class CreateDocumentRequestCommandHandler : IRequestHandler<CreateDocumentRequestCommand, Guid>
    {
        private readonly IUnitOfWork _ctx;

        public CreateDocumentRequestCommandHandler(IUnitOfWork ctx)
        {
            _ctx = ctx;
        }

        public async Task<Guid> Handle(CreateDocumentRequestCommand request, CancellationToken cancellationToken)
        {
            var entity = new DocumentRequest
            {
                StudentId = request.StudentId,
                DocumentType = request.DocumentType,
                Status = DocumentStatus.Pending,
                Remarks = request.Remarks
            };

            _ctx.DocumentRequestRepository.Add(entity);
            await _ctx.SaveChangesAsync(cancellationToken);

            return entity.Id;
        }
    }
}
