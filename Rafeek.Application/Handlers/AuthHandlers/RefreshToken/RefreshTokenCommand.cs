using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.RefreshToken
{
    public class RefreshTokenCommand : IRequest<SignResponse>
    {
        public string RefreshToken { get; set; } = null!;
    }
}
