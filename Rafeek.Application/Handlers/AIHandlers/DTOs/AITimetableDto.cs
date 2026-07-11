namespace Rafeek.Application.Handlers.AIHandlers.DTOs
{
    public class AITimetableDto
    {
        public Guid Id { get; set; }
        public Guid StudentId { get; set; }
        public string? TimetableName { get; set; }
        public string OptionName { get; set; } = null!;
        public int MaxLoad { get; set; }
        public int TotalDays { get; set; }
        public List<AITimetableItemDto> Items { get; set; } = new();
    }

    public class AITimetableItemDto
    {
        public Guid Id { get; set; }
        public string CourseName { get; set; } = null!;
        public string Type { get; set; } = null!;
        public string? SectionId { get; set; }
        public int Day { get; set; }
        public string StartTime { get; set; } = null!;
        public string EndTime { get; set; } = null!;
        public int Difficulty { get; set; }
        public int Priority { get; set; }
        public int Capacity { get; set; }
        public int AvailableSeats { get; set; }
    }
}
