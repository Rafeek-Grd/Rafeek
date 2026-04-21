using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.LearningResourceHandlers.DTOs
{
    public class LearningResourceDto
    {
        public Guid Id { get; set; }
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
        public ResourceType ResourceType { get; set; }
        public string? ResourceUrl { get; set; }
        public string? Description { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
