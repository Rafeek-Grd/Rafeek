using Rafeek.Domain.Enums;

namespace Rafeek.Application.Handlers.StudyPlanHandlers.DTOs
{
    public class StudyPlanDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public Semester Semester { get; set; }
        public Guid CourseId { get; set; }
        public string? CourseName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
