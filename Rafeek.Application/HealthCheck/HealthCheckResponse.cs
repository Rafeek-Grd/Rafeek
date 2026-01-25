namespace Rafeek.Application.HealthCheck
{
    public class HealthCheckResponse
    {
        public string OverallStatus { get; set; } = null!;
        public IEnumerable<HealthCheck> HealthChecks { get; set; } = null!;
        public string TotalDuration { get; set; } = null!;
    }
}
