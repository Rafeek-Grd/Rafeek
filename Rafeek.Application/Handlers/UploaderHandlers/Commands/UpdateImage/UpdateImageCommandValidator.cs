using System.Text.RegularExpressions;
using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateImage
{
    public class UpdateImageCommandValidator : AbstractValidator<UpdateImageCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateImageCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Value]);

            RuleFor(x => x.File)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Value]);

            // If ImageName is provided, its first digit must equal UploadPlace
            RuleFor(x => x.ImageName)
                .Must((command, imageName) =>
                {
                    if (string.IsNullOrWhiteSpace(imageName))
                        return true;

                    // find the first single digit character
                    var match = Regex.Match(imageName, @"\d");
                    if (!match.Success)
                        return false;

                    if (!int.TryParse(match.Value, out var firstDigit))
                        return false;

                    return firstDigit == command.UploadPlace;
                })
                .WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceNotValid.Value]);
        }
    }
}