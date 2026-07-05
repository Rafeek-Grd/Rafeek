using MediatR;
using Rafeek.Domain.Enums;
using System;
using System.Collections.Generic;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.BulkUpdateDocumentRequestStatus
{
    public class BulkUpdateDocumentRequestStatusCommand : IRequest<bool>
    {
        public List<Guid> RequestIds { get; set; } = new();
        public DocumentStatus Status { get; set; }
        public string? Remarks { get; set; }
    }
}
