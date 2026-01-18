using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadVideo
{
    public class UploadVideoCommandHandler : IRequestHandler<UploadVideoCommand, string>,
        IRequestHandler<UploadMultipleVideoCommand, List<string>>
    {
        private readonly IVideoValidator _videoValidator;

        public UploadVideoCommandHandler
        (
            IVideoValidator videoValidator
        )
        {
            _videoValidator = videoValidator;
        }

        public async Task<string> Handle(UploadVideoCommand request, CancellationToken cancellationToken)
        {
            var result = await _videoValidator.UploadVideo(request.File, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }

        public async Task<List<string>> Handle(UploadMultipleVideoCommand request, CancellationToken cancellationToken)
        {
            var result = await _videoValidator.UploadMultipleVideo(request.Files, request.UploadPlace);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            var finalResult = result.Result.Split(';').ToList();
            return finalResult;
        }
    }
}
