using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Common.Models.AI
{
    public class AITimetableRequestDto
    {
        [JsonPropertyName("option")]
        [JsonProperty("option")]
        public string Option { get; set; } = "balance";

        [JsonPropertyName("preferences")]
        [JsonProperty("preferences")]
        public TimetablePreferencesDto Preferences { get; set; } = new();

        [JsonPropertyName("courses")]
        [JsonProperty("courses")]
        public Dictionary<string, CourseTimetableDataDto> Courses { get; set; } = new();
    }

    public class TimetablePreferencesDto
    {
        [JsonPropertyName("buffer_minutes")]
        [JsonProperty("buffer_minutes")]
        public int BufferMinutes { get; set; } = 0;
    }

    public class CourseTimetableDataDto
    {
        [JsonPropertyName("priority")]
        [JsonProperty("priority")]
        public int Priority { get; set; }

        [JsonPropertyName("difficulty")]
        [JsonProperty("difficulty")]
        public int Difficulty { get; set; }

        [JsonPropertyName("lecture")]
        [JsonProperty("lecture")]
        public SessionDetailsDto Lecture { get; set; } = null!;

        [JsonPropertyName("sections")]
        [JsonProperty("sections")]
        public List<SessionDetailsDto> Sections { get; set; } = new();
    }

    public class SessionDetailsDto
    {
        [JsonPropertyName("id")]
        [JsonProperty("id")]
        public string? Id { get; set; }

        [JsonPropertyName("day")]
        [JsonProperty("day")]
        public int Day { get; set; }

        [JsonPropertyName("start")]
        [JsonProperty("start")]
        public string StartTime { get; set; } = null!;

        [JsonPropertyName("duration")]
        [JsonProperty("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("capacity")]
        [JsonProperty("capacity")]
        public int Capacity { get; set; }

        [JsonPropertyName("available_seats")]
        [JsonProperty("available_seats")]
        public int AvailableSeats { get; set; }
    }
}
