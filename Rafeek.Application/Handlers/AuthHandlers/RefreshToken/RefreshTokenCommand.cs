using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public string Token { get; set; } = null!;
    }
}
