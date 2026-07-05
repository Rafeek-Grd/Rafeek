using MediatR;
using Rafeek.Domain.Enums;
using System;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.UpdateDocumentRequestStatus
{
    public class UpdateDocumentRequestStatusCommand : IRequest<bool>
    {
        public Guid RequestId { get; set; }
        public DocumentStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}
