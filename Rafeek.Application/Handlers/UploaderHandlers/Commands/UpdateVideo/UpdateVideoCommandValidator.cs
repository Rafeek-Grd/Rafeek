using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using System.Text.RegularExpressions;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.UpdateVideo
{
    public class UpdateVideoCommandValidator: AbstractValidator<UpdateVideoCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public UpdateVideoCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;


            RuleFor(x => x.UploadPlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Value]);

            RuleFor(x => x.File)
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.Requried.Value]);

            // If VideoName is provided, its first digit must equal UploadPlace
            RuleFor(x => x.VideoName)
                .Must((command, videoName) =>
                {
                    if (string.IsNullOrWhiteSpace(videoName))
                        return true;

                    // find the first single digit character
                    var match = Regex.Match(videoName, @"\d");
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
