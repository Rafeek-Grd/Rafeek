using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using System.Text.RegularExpressions;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public ResetPasswordCommandValidator(IStringLocalizer<Messages> localizer, SignInManager<ApplicationUser> signInManager)
        {
            _localizer = localizer;
            _signInManager = signInManager;

            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(BeValidEmail).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value]);

            RuleFor(v => v.Token)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.TokenMessages.Required.Value])
                .MustAsync(async (command, token, cancellationToken) => 
                {
                    return await _signInManager.UserManager.Users
                        .AnyAsync(u => (u.Email == command.Email || u.TemporaryEmail == command.Email) 
                                    && u.PasswordResetToken == token 
                                    && u.PasswordResetTokenExpiredTime > DateTime.UtcNow, cancellationToken);
                })
                .WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.InvalidToken.Value]);


            RuleFor(v => v.NewPassword)
               .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordRequired.Value])
               .MinimumLength(8).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordMinLength.Value]);
               //.Must(password =>
               //{
               //    if (string.IsNullOrEmpty(password)) return false;

               //    bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");

               //    bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");

               //    bool hasDigit = Regex.IsMatch(password, @"\d");

               //    bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]");

               //    bool isValidFormat = Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]+$");

               //    return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar && isValidFormat;
               //})
               //.WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordValid.Value]);
        }

        private async Task<bool> BeValidEmail(string email, CancellationToken cancellationToken)
        {
            return await _signInManager.UserManager.Users.AnyAsync(u => u.Email == email || u.TemporaryEmail == email, cancellationToken);
        }


    }
}
