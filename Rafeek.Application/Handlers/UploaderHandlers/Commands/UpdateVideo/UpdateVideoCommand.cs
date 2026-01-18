using MediatR;
using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateVideo
{
    public class UpdateVideoCommand : IRequest<string>
    {
        public string VideoName { get; set; } = null!;
        public int UploadPlace { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
