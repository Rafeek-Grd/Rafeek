using MediatR;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.AIHandlers.Queries.GetChatHistory
{
    public class GetChatHistoryQuery : IRequest<List<ChatHistoryDto>>
    {
        public Guid? SessionId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}
