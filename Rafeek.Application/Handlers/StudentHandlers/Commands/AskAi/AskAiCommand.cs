using MediatR;
using Rafeek.Application.Handlers.StudentHandlers.DTOs;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.AskAi
{
    public class AskAiCommand : IRequest<AiChatResponseDto>
    {
        public string Question { get; set; } = string.Empty;
        
        public Guid? SessionId { get; set; }
        
        // استخدام Entity/DTO يوضح من هو المرسل "user" أو "ai"
        public List<ChatMessageDto>? History { get; set; }
    }
}
