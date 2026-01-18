using MediatR;
using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadVideo
{
    public class UploadVideoCommand: IRequest<string>
    {
        public int UploadPlace { get; set; }
        public IFormFile File { get; set; } = null!;
    }

    public class UploadMultipleVideoCommand: IRequest<List<string>>
    {
        public int UploadPlace { get; set; }
        public List<IFormFile> Files { get; set; } = null!;
    }
}
