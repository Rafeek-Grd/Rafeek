using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AuthHandlers.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;

        public ResetPasswordCommandValidator(IStringLocalizer<Messages> localizer)
        {
            _localizer = localizer;

            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value]);

            RuleFor(v => v.Token)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.TokenMessages.Required.Value]);

            RuleFor(v => v.NewPassword)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordRequired.Value])
                .MinimumLength(8).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordMinLength.Value]);

            RuleFor(v => v.ConfirmPassword)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordRequired.Value])
                .Equal(v => v.NewPassword).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordConfirmNotEqual.Value]);
        }
    }
}
