using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AnnouncementHandlers.DTOs;

namespace Rafeek.Application.Handlers.AnnouncementHandlers.Queries.GetAnnouncementsPaginated
{
    public class GetAnnouncementsPaginatedQuery : IRequest<PagginatedResult<AnnouncementDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchTerm { get; set; }
        public bool? IsSent { get; set; }
        public bool? IsDeactivated { get; set; }
    }
}
