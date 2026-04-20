using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class ChatbotQuery : BaseEntity
    {
        public Guid StudentId { get; set; }
        public Guid SessionId { get; set; }
        public string Query { get; set; } = null!;
        public string Response { get; set; } = null!;

        public Student Student { get; set; } = null!;
        
        [System.ComponentModel.DataAnnotations.Schema.ForeignKey("SessionId")]
        public ChatSession ChatSession { get; set; } = null!;
    }
}
