using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.ForegetPassword
{
    public class ForegetPasswordCommandValidator : AbstractValidator<ForegetPasswordCommand>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public ForegetPasswordCommandValidator(SignInManager<ApplicationUser> signInManager, IStringLocalizer<Messages> localizer)
        {
            _signInManager = signInManager;
            _localizer = localizer;

            RuleFor(x => x.Email)
                .NotNull().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .NotEmpty().EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(EmailExists).WithMessage(_localizer[LocalizationKeys.UserMessages.EmailNotFoundBefore.Value]);
        }

        private async Task<bool> EmailExists(string email, CancellationToken cancellationToken)
        {
            var user = await _signInManager.UserManager.FindByEmailAsync(email);
            return user?.Email != null;
        }
    }
}