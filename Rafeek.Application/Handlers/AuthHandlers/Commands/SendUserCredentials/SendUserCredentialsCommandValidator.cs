using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.SendUserCredentials
{
    public class SendUserCredentialsCommandValidator : AbstractValidator<SendUserCredentialsCommand>
    {
        public SendUserCredentialsCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(x => x.Email)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress()
                .WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value]);

            RuleFor(x => x.Password)
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.PasswordRequired.Value]);
        }
    }
}
