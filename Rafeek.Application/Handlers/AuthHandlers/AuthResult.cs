namespace Rafeek.Application.Handlers.AuthHandlers
{
    public class AuthResult
    {
        public string Token { get; set; } = null!;
        public string TokenType { get; set; } = null!;
        public DateTime ExpiresIn { get; set; }
        public string RefreshToken { get; set; } = null!;
    }
}
