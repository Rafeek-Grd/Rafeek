using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.DocumentHandlers.DTOs;
using Rafeek.Domain.Enums;
using System;

namespace Rafeek.Application.Handlers.DocumentHandlers.Queries.GetDocumentRequestsPaginated
{
    public class GetDocumentRequestsPaginatedQuery : IRequest<PagginatedResult<DocumentRequestDto>>
    {
        public DocumentStatus? Status { get; set; }
        public string? DocumentType { get; set; }
        public string? SearchTerm { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? StudentId { get; set; }
        public Guid? AdvisorId { get; set; }

        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
