using MediatR;
using Rafeek.Application.Handlers.AIHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi
{
    public class AskAiCommand : IRequest<AiChatResponseDto>
    {
        public string Question { get; set; } = string.Empty;
        public Guid? SessionId { get; set; }
        public List<ChatMessageDto>? History { get; set; }
    }
}
