namespace Rafeek.Application.Handlers.AIHandlers.DTOs
{
    public class ChatHistoryDto
    {
        public Guid Id { get; set; }
        public Guid SessionId { get; set; }
        public string Question { get; set; } = null!;
        public string Answer { get; set; } = null!;
        public DateTime AskedAt { get; set; }
    }
}
