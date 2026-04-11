using MediatR;

namespace Rafeek.Application.Handlers.StudentHandlers.Commands.SendRequestForAdvismentGuide
{
    public class SendRequestForAdvismentGuideCommand: IRequest<string>
    {
        public Guid StudentId { get; set; }
        public string Title { get; set; } = null!;
        public string Description { get; set; } = null!;
    }
}
