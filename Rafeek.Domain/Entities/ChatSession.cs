using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class ChatSession : BaseEntity
    {
        public Guid UserId { get; set; }
        public string Title { get; set; } = null!;

        public ICollection<ChatbotQuery> ChatbotQueries { get; set; } = new HashSet<ChatbotQuery>();
    }
}
