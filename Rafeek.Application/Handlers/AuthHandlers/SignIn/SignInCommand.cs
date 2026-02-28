using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.SignIn
{
    public class SignInCommand : IRequest<SignResponse>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
        public string? FbToken { get; set; }
        public bool IsAndroidDevice { get; set; }
        public bool IsIosDevice { get; set; }
    }
}
