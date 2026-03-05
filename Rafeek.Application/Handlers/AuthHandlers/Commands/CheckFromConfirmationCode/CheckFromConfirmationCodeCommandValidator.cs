using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.CheckFromConfirmationCode
{
    public class CheckFromConfirmationCodeCommandValidator : AbstractValidator<CheckFromConfirmationCodeCommand>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public CheckFromConfirmationCodeCommandValidator(SignInManager<ApplicationUser> signInManager, IStringLocalizer<Messages> localizer)
        {
            _signInManager = signInManager;
            _localizer = localizer;

            RuleFor(x => x.ConfirmationCode)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.ResetTokenInvalid]);

            RuleFor(x => x.Email)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(BeAValidEmail).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailNotFound.Value]);
        }

        private Task<bool> BeAValidEmail(string email, CancellationToken cancellationToken)
        {
            return _signInManager.UserManager.Users.AnyAsync(u => u.Email == email || u.TemporaryEmail == email, cancellationToken);
        }
    }
}
