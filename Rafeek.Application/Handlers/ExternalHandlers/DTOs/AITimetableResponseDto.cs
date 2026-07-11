using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class AITimetableResponseDto
    {
        [JsonProperty("stats")]
        [JsonPropertyName("stats")]
        public TimetableStatsDto Stats { get; set; } = null!;

        [JsonProperty("schedule")]
        [JsonPropertyName("schedule")]
        public List<ScheduledItemDto> Schedule { get; set; } = new();
    }

    public class TimetableStatsDto
    {
        [JsonProperty("max_load")]
        [JsonPropertyName("max_load")]
        public int MaxLoad { get; set; }

        [JsonProperty("total_days")]
        [JsonPropertyName("total_days")]
        public int TotalDays { get; set; }

        [JsonProperty("option_name")]
        [JsonPropertyName("option_name")]
        public string OptionName { get; set; } = null!;
    }

    public class ScheduledItemDto
    {
        [JsonProperty("course")]
        [JsonPropertyName("course")]
        public string Course { get; set; } = null!;

        [JsonProperty("type")]
        [JsonPropertyName("type")]
        public string Type { get; set; } = null!;

        [JsonProperty("section_id")]
        [JsonPropertyName("section_id")]
        public string? SectionId { get; set; }

        [JsonProperty("day")]
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonProperty("start_time")]
        [JsonPropertyName("start_time")]
        public string StartTime { get; set; } = null!;

        [JsonProperty("end_time")]
        [JsonPropertyName("end_time")]
        public string EndTime { get; set; } = null!;

        [JsonProperty("difficulty")]
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("priority")]
        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonProperty("capacity")]
        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonProperty("available_seats")]
        [JsonPropertyName("available_seats")]
        public int AvailableSeats { get; set; }
    }
}
