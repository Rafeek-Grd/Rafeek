using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ForegetPassword
{
    public class ForegetPasswordCommand: IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
    }
}
