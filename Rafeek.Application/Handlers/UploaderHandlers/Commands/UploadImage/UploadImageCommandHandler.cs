using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadImage
{
    public class UploadImageCommandHandler : IRequestHandler<UploadImageCommand, string>, IRequestHandler<UploadMultipleImageCommand, List<string>>
    {
        private readonly IImageValidator _imageValidator;

        public UploadImageCommandHandler
        (
            IImageValidator imageValidator
        )
        {
            _imageValidator = imageValidator;
        }

        public async Task<string> Handle(UploadImageCommand request, CancellationToken cancellationToken)
        {
            var result = await _imageValidator.UploadImage(request.File, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }

        public async Task<List<string>> Handle(UploadMultipleImageCommand request, CancellationToken cancellationToken)
        {
            var result = await _imageValidator.UploadMultipleImage(request.Files, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            var finalResult = result.Result.Split(';').ToList();
            return finalResult;
        }
    }
}
