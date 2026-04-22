using Newtonsoft.Json;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class AITimetableResponseDto
    {
        [JsonProperty("stats")]
        public TimetableStatsDto Stats { get; set; } = null!;

        [JsonProperty("schedule")]
        public List<ScheduledItemDto> Schedule { get; set; } = new();
    }

    public class TimetableStatsDto
    {
        [JsonProperty("max_load")]
        public int MaxLoad { get; set; }

        [JsonProperty("total_days")]
        public int TotalDays { get; set; }

        [JsonProperty("option_name")]
        public string OptionName { get; set; } = null!;
    }

    public class ScheduledItemDto
    {
        [JsonProperty("course")]
        public string Course { get; set; } = null!;

        [JsonProperty("type")]
        public string Type { get; set; } = null!;

        [JsonProperty("section_id")]
        public string? SectionId { get; set; }

        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; } = null!;

        [JsonProperty("end_time")]
        public string EndTime { get; set; } = null!;

        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        [JsonProperty("available_seats")]
        public int AvailableSeats { get; set; }
    }
}
