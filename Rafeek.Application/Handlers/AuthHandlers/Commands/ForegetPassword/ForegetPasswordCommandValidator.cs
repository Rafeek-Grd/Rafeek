using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ForegetPassword
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
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(IsValidEmail).WithMessage(_localizer[LocalizationKeys.UserMessages.EmailNotFoundBefore.Value])
                .MustAsync(IsActivatedUniversityEmail).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailNotActivated.Value]);
        }
        public Task<bool> IsValidEmail(string email, CancellationToken cancellationToken)
        {
            return _signInManager.UserManager.Users.AnyAsync(u => u.Email == email || u.TemporaryEmail == email, cancellationToken);
        }

        private async Task<bool> IsActivatedUniversityEmail(string email, CancellationToken cancellationToken)
        {
            return await _signInManager.UserManager.Users.AnyAsync(u => (u.Email == email || u.TemporaryEmail == email) && u.IsUniversityEmailActivated, cancellationToken);
        }
    }
}