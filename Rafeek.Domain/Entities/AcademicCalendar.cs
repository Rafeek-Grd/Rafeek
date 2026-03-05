using Rafeek.Domain.Common;

namespace Rafeek.Domain.Entities
{
    public class AcademicCalendar : BaseEntity
    {
        public string EventName { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime EventDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsAllDay { get; set; }
        public string? Location { get; set; }
    }
}
