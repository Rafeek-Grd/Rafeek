using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.ForegetPassword
{
    public class ForegetPasswordCommand: IRequest<string>
    {
        public string Email { get; set; } = string.Empty;
    }
}
