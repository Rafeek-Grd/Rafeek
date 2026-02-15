using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class StudentSupport : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StudentSupportStatus StudentSupportStatus { get; set; }

        public Guid StudentId { get; set; }
        public Student Student { get; set; } = null!;
    }
}
