using MediatR;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.DownloadFile
{
    public class DownloadFileCommand: IRequest<string>
    {
        public int FilePlace { get; set; }
        public string FileName { get; set; } = null!;
    }
}
