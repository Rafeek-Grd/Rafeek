using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Services;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.UploaderHandlers.Commands.DownloadFile
{
    public class DownloadFileCommandValidator: AbstractValidator<DownloadFileCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IFileValidator _fileValidator;

        public DownloadFileCommandValidator(IStringLocalizer<Messages> localizer,IFileValidator fileValidator)
        {
            _localizer = localizer;
            _fileValidator = fileValidator;

            RuleFor(x => x.FilePlace)
                .NotNull().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.PalceRequried.Value]);

            RuleFor(x => new { x.FileName, x.FilePlace })
                .NotNull().NotEmpty().WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.NameRequried.Value])
                .Must((request) => _fileValidator.IsValidFile(request.FileName, UploadPaths.GetPath(request.FilePlace)))
                .WithMessage(x => _localizer[LocalizationKeys.UploadFileMessages.FileNotFound.Value]);

        }
    }
}
