using MediatR;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadAudio
{
    public class UploadAudioCommandHandler : IRequestHandler<UploadAudioCommand, string>
    {
        private readonly IAudioValidator _audioValidator;

        public UploadAudioCommandHandler
        (
            IAudioValidator audioValidator
        )
        {
            _audioValidator = audioValidator;
        }

        public async Task<string> Handle(UploadAudioCommand request, CancellationToken cancellationToken)
        {
            var result = await _audioValidator.UploadAudio(request.File, request.UploadPlace, cancellationToken);

            if (!result.Uploaded)
            {
                throw new BadRequestException(result.Result);
            }
            return result.Result;
        }
    }
}
