using MediatR;
using Rafeek.Application.Common.Models;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetChatHistory
{
    public class GetChatHistoryQuery : IRequest<PagginatedResult<ChatHistoryDto>>
    {
        public Guid? SessionId { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
