using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class AITimetableRequestDto
    {
        [JsonPropertyName("option")]
        public string Option { get; set; } = "balance";

        [JsonPropertyName("preferences")]
        public TimetablePreferencesDto Preferences { get; set; } = new();

        [JsonPropertyName("courses")]
        public Dictionary<string, CourseTimetableDataDto> Courses { get; set; } = new();
    }

    public class TimetablePreferencesDto
    {
        [JsonPropertyName("buffer_minutes")]
        public int BufferMinutes { get; set; } = 0;
    }

    public class CourseTimetableDataDto
    {
        [JsonPropertyName("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("difficulty")]
        public int Difficulty { get; set; }

        [JsonPropertyName("lecture")]
        public SessionDetailsDto Lecture { get; set; } = null!;

        [JsonPropertyName("sections")]
        public List<SessionDetailsDto> Sections { get; set; } = new();
    }

    public class SessionDetailsDto
    {
        [JsonPropertyName("id")]
        public string? Id { get; set; }

        [JsonPropertyName("day")]
        public int Day { get; set; }

        [JsonPropertyName("start")]
        [JsonProperty("start")]
        public string StartTime { get; set; } = null!;

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("available_seats")]
        public int AvailableSeats { get; set; }
    }
}
