using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ResetPassword
{
    public class ResetPasswordCommand : IRequest<ResetPasswordResponse>
    {
        public string Email { get; set; } = null!;
        public string Token { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}
