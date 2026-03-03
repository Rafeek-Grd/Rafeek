using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.SendUserCredentials
{
    public class SendUserCredentialsCommand : IRequest<string>
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }
}
