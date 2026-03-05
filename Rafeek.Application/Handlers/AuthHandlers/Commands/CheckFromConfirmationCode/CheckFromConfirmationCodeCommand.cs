using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.CheckFromConfirmationCode
{
    public class CheckFromConfirmationCodeCommand : IRequest<CheckFromConfirmationCodeResponse>
    {
        public string Email { get; set; } = string.Empty;
        public string ConfirmationCode { get; set; } = string.Empty;
    }
}
