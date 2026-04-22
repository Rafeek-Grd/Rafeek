using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AssignmentHandlers.DTOs;

namespace Rafeek.Application.Handlers.AssignmentHandlers.Queries.GetAssignmentsBySection
{
    public class GetAssignmentsBySectionQueryPaginated : IRequest<PagginatedResult<AssignmentDto>>
    {
        public Guid SectionId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
