namespace Rafeek.Application.Handlers.GPAHandlers.DTOs
{
    public class GPASimulatorLogDto
    {
        public Guid Id { get; set; }
        public float ExpectedGPA { get; set; }
        public float PredictedCGPA { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
