using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.AIHandlers.DTOs
{
    public class ChatMessageDto
    {
        [JsonPropertyName("role")]
        public string Role { get; set; } = string.Empty; // "user" or "ai" or "assistant"

        [JsonPropertyName("content")]
        public string Content { get; set; } = string.Empty;
    }
}
