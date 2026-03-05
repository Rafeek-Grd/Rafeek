using MediatR;
using Rafeek.Application.Handlers.AuthHandlers.Commands;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.RefreshToken
{
    public class RefreshTokenCommand : IRequest<AuthResult>
    {
        public string Token { get; set; } = null!;
    }
}
