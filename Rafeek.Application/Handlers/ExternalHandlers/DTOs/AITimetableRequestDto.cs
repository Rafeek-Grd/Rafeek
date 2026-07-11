using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class AITimetableRequestDto
    {
        [JsonProperty("option")]
        [JsonPropertyName("option")]
        public string Option { get; set; } = "balance";

        [JsonProperty("preferences")]
        [JsonPropertyName("preferences")]
        public TimetablePreferencesDto Preferences { get; set; } = new();

        [JsonProperty("courses")]
        [JsonPropertyName("courses")]
        public Dictionary<string, CourseTimetableDataDto> Courses { get; set; } = new();
    }

    public class TimetablePreferencesDto
    {
        [JsonProperty("buffer_minutes")]
        [JsonPropertyName("buffer_minutes")]
        public int BufferMinutes { get; set; } = 0;
    }

    public class CourseTimetableDataDto
    {
        [JsonProperty("priority")]
        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonProperty("difficulty")]
        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; }

        [JsonProperty("lecture")]
        [JsonPropertyName("lecture")]
        public SessionDetailsDto Lecture { get; set; } = null!;

        [JsonProperty("sections")]
        [JsonPropertyName("sections")]
        public List<SessionDetailsDto> Sections { get; set; } = new();
    }

    public class SessionDetailsDto
    {
        [JsonProperty("id")]
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonProperty("day")]
        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonProperty("start")]
        [JsonPropertyName("start")]
        public string StartTime { get; set; } = null!;

        [JsonProperty("duration")]
        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonProperty("capacity")]
        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonProperty("available_seats")]
        [JsonPropertyName("available_seats")]
        public int AvailableSeats { get; set; }
    }
}
