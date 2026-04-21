using MediatR;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.CreateDocumentRequest
{
    public class CreateDocumentRequestCommand : IRequest<Guid>
    {
        public Guid StudentId { get; set; }
        public string DocumentType { get; set; } = null!;
        public string? Remarks { get; set; }
    }
}
