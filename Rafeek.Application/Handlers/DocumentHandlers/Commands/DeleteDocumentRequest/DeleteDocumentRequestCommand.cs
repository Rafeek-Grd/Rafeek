using MediatR;

namespace Rafeek.Application.Handlers.DocumentHandlers.Commands.DeleteDocumentRequest
{
    public class DeleteDocumentRequestCommand: IRequest<string>
    {
        public Guid? DocumentRequestId { get; set; }
    }
}
