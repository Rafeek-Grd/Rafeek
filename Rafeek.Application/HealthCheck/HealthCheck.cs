namespace Rafeek.Application.HealthCheck
{
    public class HealthCheck
    {
        public string Status { get; set; } = null!;
        public string Component { get; set; } = null!;
        public string Description { get; set; } = null!;
        public string Duration { get; set; } = null!;
    }
}
