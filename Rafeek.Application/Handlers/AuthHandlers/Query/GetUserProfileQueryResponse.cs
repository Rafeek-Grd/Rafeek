namespace Rafeek.Application.Handlers.AuthHandlers.Query
{
    public class GetUserProfileQueryResponse
    {
        public Guid Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string? TemporaryEmail { get; set; }
        public bool IsUniversityEmailActivated { get; set; }
        public string? ProfilePictureUrl { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }
        public string NationalId { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
