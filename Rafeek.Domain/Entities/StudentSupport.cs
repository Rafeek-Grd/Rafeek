using Rafeek.Domain.Common;
using Rafeek.Domain.Enums;

namespace Rafeek.Domain.Entities
{
    public class StudentSupport : BaseEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Email { get; set; }
        public StudentSupportStatus StudentSupportStatus { get; set; }
        public StudentSupportType TicketType { get; set; }

        public Guid? StudentId { get; set; }
        public Student? Student { get; set; }
    }
}
