namespace Rafeek.Application.Handlers.AIHandlers.DTOs
{
    public class AiSessionDto
    {
        public Guid SessionId { get; set; }
        public string? SessionTitle { get; set; }
        public DateTime LastMessageAt { get; set; }
        public int MessagesCount { get; set; }
    }
}
