using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ActivateUniversityEmail
{
    public class ActivateUniversityEmailCommand : IRequest<string>
    {
        public string Email { get; set; } = null!;
    }
}
