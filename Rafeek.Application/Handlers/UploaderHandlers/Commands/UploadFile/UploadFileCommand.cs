using MediatR;
using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadFile
{
    public class UploadFileCommand: IRequest<string>
    {
        public int UploadPlace { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
