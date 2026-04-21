namespace Rafeek.Application.Handlers.CareerHandlers.DTOs
{
    public class CareerSuggestionDto
    {
        public Guid Id { get; set; }
        public string CareerPath { get; set; } = null!;
        public string Justification { get; set; } = null!;
        public DateTime CreatedAt { get; set; }
    }
}
