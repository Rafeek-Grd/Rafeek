using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ActivateUniversityEmail
{
    public class ActivateUniversityEmailCommandValidator : AbstractValidator<ActivateUniversityEmailCommand>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public ActivateUniversityEmailCommandValidator(IStringLocalizer<Messages> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;

            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .Must(email =>
                {
                    if (string.IsNullOrEmpty(email)) return false;
                    return email.EndsWith("@std.mans.edu.eg", StringComparison.OrdinalIgnoreCase) ||
                           email.EndsWith("@mans.edu.eg", StringComparison.OrdinalIgnoreCase);
                })
                .WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailDomainInvalid.Value])
                .MustAsync(IsAlreadyActivatedEmail).WithMessage(_localizer[LocalizationKeys.UserMessages.EmailAlreadyActivated.Value]);

            RuleFor(v => v.ConfirmationCode)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.ResetTokenInvalid.Value]);
        }


        private Task<bool> IsAlreadyActivatedEmail(string email, CancellationToken cancellationToken)
        {
            return _signInManager.UserManager.Users.AnyAsync(u => u.Email == email && u.IsUniversityEmailActivated, cancellationToken);
        }
    }
}
