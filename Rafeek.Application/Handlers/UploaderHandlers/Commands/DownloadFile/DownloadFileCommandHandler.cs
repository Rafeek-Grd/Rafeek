using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.DownloadFile
{
    public class DownloadFileCommandHandler : IRequestHandler<DownloadFileCommand, string>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IFileValidator _fileValidator;

        public DownloadFileCommandHandler
        (
            IStringLocalizer<Messages> localizer,
            IFileValidator fileValidator    
        )
        {
            _localizer = localizer;
            _fileValidator = fileValidator;
        }

        public async Task<string> Handle(DownloadFileCommand request, CancellationToken cancellationToken)
        {
            var file = await _fileValidator.DownloadFile(request.FilePlace, request.FileName);
            if (!file.Item1)
                return _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value];
            else
                return file.Item2;
        }
    }
}
