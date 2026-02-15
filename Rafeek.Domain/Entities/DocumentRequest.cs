using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class DocumentRequest : BaseEntity
    {
        public string DocumentType { get; set; } = string.Empty;
        public DocumentStatus Status { get; set; }
        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
