using FluentValidation;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;

namespace Rafeek.Application.Handlers.AuthHandlers.RefreshToken
{
    public class RefreshTokenCommandValidator : AbstractValidator<RefreshTokenCommand>
    {
        public RefreshTokenCommandValidator(IStringLocalizer<Messages> localizer)
        {
            RuleFor(v => v.RefreshToken)
                .NotNull()
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.TokenMessages.NotValid.Value]);
        }
    }
}
