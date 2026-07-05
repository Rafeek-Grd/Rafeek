using Rafeek.Application.Handlers.StudentHandlers.DTOs;
using System;

namespace Rafeek.Application.Handlers.DocumentHandlers.DTOs
{
    public class DocumentRequestDto
    {
        public Guid Id { get; set; }
        public string DocumentType { get; set; } = null!;
        public string Status { get; set; } = null!; // Pending, Approved, Rejected
        public string? Remarks { get; set; }
        public string? Topic { get; set; }
        public string? AttachmentUrl { get; set; }
        public Guid StudentId { get; set; }
        public StudentDto Student { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
