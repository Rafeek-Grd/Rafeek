using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadAudio
{
    public class UploadAudioCommandValidator: AbstractValidator<UploadAudioCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UploadAudioCommandValidator( IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Value]);

            RuleFor(x => x.File)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Value]);
        }
    }
}
