using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class ChatbotQuery : BaseEntity
    {
        public Guid StudentId { get; set; }
        public string Query { get; set; } = null!;
        public string Response { get; set; } = null!;

        public Student Student { get; set; } = null!;
    }
}
