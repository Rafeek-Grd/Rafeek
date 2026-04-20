namespace Rafeek.Application.Common.Options
{
    public class AiIntegrationSettings
    {
        public string BaseUrl { get; set; } = string.Empty;
        public string TimeTableBaseUrl { get; set; } = string.Empty;
        public AiServices Services { get; set; } = null!;
    }

    public class AiServices
    {
        public string AiRecommendation { get; set; } = string.Empty;
        public string AskChatbot { get; set; } = string.Empty;
        public string GetRootOfChatbot { get; set; } = string.Empty;
        public string GenerateTimeTable { get; set; } = string.Empty;
    }
}
