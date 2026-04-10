using Rafeek.Domain.Enums;
using System;

namespace Rafeek.Application.Handlers.AdvisorHandlers.DTOs
{
    public class GuidanceRequestDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public StudentSupportStatus StudentSupportStatus { get; set; }
        public Guid StudentId { get; set; }
        public string StudentUniversityCode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
