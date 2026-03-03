using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInCommandValidator(IStringLocalizer<Messages> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _signInManager = signInManager;

            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull()
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress()
                .WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(IsActivatedUniversityEmail).WithMessage(localizer[LocalizationKeys.GlobalValidationMessages.EmailNotActivated.Value]);

            RuleFor(v => v.Password)
                .NotNull()
                .NotEmpty()
                .WithMessage(localizer[LocalizationKeys.UserMessages.PasswordRequired.Value]);
        }

        private async Task<bool> IsActivatedUniversityEmail(string email, CancellationToken cancellationToken)
        {
            return await _signInManager.UserManager.Users
                .AnyAsync(u => u.Email == email && u.IsUniversityEmailActivated, cancellationToken);
        }
    }
}
