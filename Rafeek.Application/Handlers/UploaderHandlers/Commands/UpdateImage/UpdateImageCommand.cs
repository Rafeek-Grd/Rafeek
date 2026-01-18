using MediatR;
using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateImage
{
    public class UpdateImageCommand: IRequest<string>
    {
        public string ImageName { get; set; } = null!;
        public int UploadPlace { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
