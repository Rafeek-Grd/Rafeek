using System;

namespace Rafeek.Application.Handlers.StudentHandlers.DTOs
{
    public class ScheduleItemDto
    {
        public Guid CourseId { get; set; }
        public string CourseCode { get; set; } = null!;
        public string CourseTitle { get; set; } = null!;
        public Guid SectionId { get; set; }
        public string Day { get; set; } = null!;
        public string Time { get; set; } = null!;
        public string Status { get; set; } = null!;
    }
}
