using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetAiSessions
{
    public class GetAiSessionsQuery : IRequest<PagginatedResult<AiSessionDto>>
    {
        public int PageNumber { get; set; } = -1;
        public int PageSize { get; set; } = 20;
    }
}
