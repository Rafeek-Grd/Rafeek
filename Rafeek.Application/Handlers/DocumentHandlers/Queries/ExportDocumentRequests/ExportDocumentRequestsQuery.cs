using MediatR;
using Rafeek.Domain.Enums;
using System;

namespace Rafeek.Application.Handlers.DocumentHandlers.Queries.ExportDocumentRequests
{
    public class ExportDocumentRequestsQuery : IRequest<byte[]>
    {
        public DocumentStatus? Status { get; set; }
        public string? DocumentType { get; set; }
        public string? SearchTerm { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? AdvisorId { get; set; }
    }
}
