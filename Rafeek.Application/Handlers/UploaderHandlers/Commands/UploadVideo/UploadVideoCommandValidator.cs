using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadVideo
{
    public class UploadVideoCommandValidator: AbstractValidator<UploadVideoCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UploadVideoCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Value]);

            RuleFor(x => x.File)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Value]);
        }
    }

    public class UploadMultipleVideoCommandValidator: AbstractValidator<UploadMultipleVideoCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UploadMultipleVideoCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Key]);

            RuleFor(x => x.Files)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Key]);
        }
    }
}
