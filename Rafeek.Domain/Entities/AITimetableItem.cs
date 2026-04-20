using System;
using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AITimetableItem : BaseEntity
    {
        public Guid TimetableId { get; set; }
        public AITimetable Timetable { get; set; } = null!;

        public string CourseName { get; set; } = null!;
        public string Type { get; set; } = null!; // LECTURE or SECTION
        public string? SectionId { get; set; } // The ID returned by the AI

        public int Day { get; set; }
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;

        public int Difficulty { get; set; }
        public int Priority { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
    }
}
