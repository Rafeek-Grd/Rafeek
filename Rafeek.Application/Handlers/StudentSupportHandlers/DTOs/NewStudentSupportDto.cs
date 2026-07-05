using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudentSupportHandlers.DTOs
{
    public class NewStudentSupportDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public StudentSupportType TicketType { get; set; }
        public StudentSupportStatus StudentSupportStatus { get; set; }
        public bool IsActive { get; set; }
        public bool IsDeleted { get; set; }
    }
}
