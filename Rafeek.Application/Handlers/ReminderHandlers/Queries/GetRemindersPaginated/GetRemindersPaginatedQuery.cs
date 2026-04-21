using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.ReminderHandlers.DTOs;

namespace Rafeek.Application.Handlers.ReminderHandlers.Queries.GetRemindersPaginated
{
    public class GetRemindersPaginatedQuery : IRequest<PagginatedResult<ReminderDto>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
        public string? SearchTerm { get; set; }
        public bool? IsCompleted { get; set; }
    }
}
