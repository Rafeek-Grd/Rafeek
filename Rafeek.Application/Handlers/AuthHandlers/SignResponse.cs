namespace Rafeek.Application.Handlers.AuthHandlers
{
    public class SignResponse
    {
        public string Id { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public int Role { get; set; }
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public string TokenType { get; set; } = null!;
        public object ExpiresIn { get; set; } = null!;
    }
}
