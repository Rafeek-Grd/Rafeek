using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Rafeek.Application.Handlers.ExternalHandlers.DTOs
{
    public class AIRecommendationDto
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;

        [JsonPropertyName("source")]
        public string Source { get; set; } = null!;

        [JsonPropertyName("dominant_track")]
        public string DominantTrack { get; set; } = null!;

        [JsonPropertyName("track_confidence")]
        public string TrackConfidence { get; set; } = null!;

        [JsonPropertyName("track_reasoning")]
        public string TrackReasoning { get; set; } = null!;

        [JsonPropertyName("recommendations")]
        public List<CourseRecommendationDto> Recommendations { get; set; } = new();
    }

    public class CourseRecommendationDto
    {
        [JsonPropertyName("course_code")]
        public string CourseCode { get; set; } = null!;

        [JsonPropertyName("course_name")]
        public string CourseName { get; set; } = null!;

        [JsonPropertyName("score")]
        public double Score { get; set; }

        [JsonPropertyName("confidence")]
        public string Confidence { get; set; } = null!;
    }
}
