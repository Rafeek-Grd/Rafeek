using MediatR;
using Microsoft.AspNetCore.Http;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadAudio
{
    public class UploadAudioCommand: IRequest<string>
    {
        public int UploadPlace { get; set; }
        public IFormFile File { get; set; } = null!;
    }
}
