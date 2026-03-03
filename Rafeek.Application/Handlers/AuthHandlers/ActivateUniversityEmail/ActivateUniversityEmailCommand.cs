using MediatR;

namespace Rafeek.Application.Handlers.AuthHandlers.ActivateUniversityEmail
{
    public class ActivateUniversityEmailCommand : IRequest<string>
    {
        public string Email { get; set; } = null!;
    }
}
