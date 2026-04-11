using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.AdvisorHandlers.DTOs
{
    public class GuidanceRequestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StudentSupportStatus StudentSupportStatus { get; set; }
        public Guid StudentId { get; set; }
        public StudentDto Student { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
