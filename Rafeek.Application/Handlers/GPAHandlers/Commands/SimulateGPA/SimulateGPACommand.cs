using MediatR;

namespace Rafeek.Application.Handlers.GPAHandlers.Commands.SimulateGPA
{
    public class SimulateGPACommand : IRequest<float>
    {
        public Guid StudentId { get; set; }
        public float ExpectedGPA { get; set; }
    }
}
