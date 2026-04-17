using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AdvisorHandlers.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdvisorHandlers.Queries.GetAllGuidenceSupportRequests
{
    public class GetAllGuidenceSupportRequestsPagginatedQuery: IRequest<PagginatedResult<GuidanceRequestDto>>
    {
        public StudentSupportStatus? RequestStatus { get; set; }
        // search by user fullname or email
        public string? SearchTerm { get; set; }
        public Guid? DepartmentId { get; set; }
        public Guid? AdvisorId { get; set; }
        public Guid? StudentId { get; set; }

        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
