using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AuthHandlers.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress()
                .WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value]);

            RuleFor(v => v.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.PasswordRequired.Value]);
        }
    }
}
