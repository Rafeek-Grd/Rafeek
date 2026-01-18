using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UploadImage
{
    public class UploadImageCommandValidator : AbstractValidator<UploadImageCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UploadImageCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Key]);

            RuleFor(x => x.File)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Key]);
        }
    }

    public class UploadMultipleImageCommandValidator : AbstractValidator<UploadMultipleImageCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UploadMultipleImageCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Key]);

            RuleFor(x => x.Files)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Key]);
        }
    }
}