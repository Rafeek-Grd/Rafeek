namespace Rafeek.Application.Handlers.AIHandlers.DTOs
{
    public class AiChatResponseDto
    {
        public string Answer { get; set; } = string.Empty;
        public Guid SessionId { get; set; }
    }
}
