using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Services;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateVideo
{
    public class UpdateVideoCommandHandler : IRequestHandler<UpdateVideoCommand, string>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IVideoValidator _videoValidator;

        public UpdateVideoCommandHandler
        (
            IStringLocalizer<Messages> localizer,
            IVideoValidator videoValidator    
        )
        {
            _localizer = localizer;
            _videoValidator = videoValidator;
        }

        public async Task<string> Handle(UpdateVideoCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.VideoName))
            {
                try
                {
                    await _videoValidator.DeleteVideo(request.VideoName, request.UploadPlace);
                }
                catch
                {
                    throw new BadRequestException(_localizer[LocalizationKeys.UploadFileMessages.FileFaildToDeleted.Value]);
                }
            }

            var result = await _videoValidator.UploadVideo(request.File, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }
    }
}
