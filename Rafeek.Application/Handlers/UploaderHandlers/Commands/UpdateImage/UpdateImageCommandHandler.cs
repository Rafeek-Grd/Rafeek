using MediatR;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateImage
{
    public class UpdateImageCommandHandler : IRequestHandler<UpdateImageCommand, string>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IImageValidator _imageValidator;

        public UpdateImageCommandHandler
        (
            IStringLocalizer<Messages> localizer,
            IImageValidator imageValidator
        )
        {
            _localizer = localizer;
            _imageValidator = imageValidator;
        }

        public async Task<string> Handle(UpdateImageCommand request, CancellationToken cancellationToken)
        {
            if (!string.IsNullOrWhiteSpace(request.ImageName))
            {
                try
                {
                    await _imageValidator.DeleteImage(request.ImageName, request.UploadPlace);
                }
                catch
                {
                    throw new BadRequestException(_localizer[LocalizationKeys.UploadFileMessages.FileFaildToDeleted.Value]);
                }
            }

            var result = await _imageValidator.UploadImage(request.File, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }
    }
}
