using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;

namespace Rafeek.Application.Handlers.AuthHandlers.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpCommandValidator(IStringLocalizer<Messages> localizer, UserManager<ApplicationUser> userManager)
        {
            _localizer = localizer;
            _userManager = userManager;

            RuleFor(v => v.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().NotNull().WithMessage(_localizer[LocalizationKeys.UserMessages.FullNameRequired.Value]);

            RuleFor(v => v.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().NotNull().WithMessage(_localizer[LocalizationKeys.UserMessages.PhoneRequired.Value])
                .MaximumLength(20).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneInvalid.Value])
                .Matches(@"^\s*0?\d+\s*$").WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneInvalid.Value])
                .MustAsync(PhoneIsNotExist).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneNumberExist.Value]);

            RuleFor(v => v.Email)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(EmailIsNotExist).WithMessage(_localizer[LocalizationKeys.UserMessages.EmailAlreadyExistedbefore.Value]);

            RuleFor(v => v.NationalNumber)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.NationalNumberRequired.Value])
                .MustAsync(NationalNumberIsNotExist).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.NationalNumberExist.Value]);

             RuleFor(x => x.Gender)
                .Cascade(CascadeMode.Stop)
                .Must(g => !g.HasValue || Enum.IsDefined(typeof(GenderType), g.Value))
                .WithMessage(_localizer[LocalizationKeys.UserMessages.GenderIsNotValid.Value]);

            RuleFor(v => v.Password)
            .Cascade(CascadeMode.Stop)
            .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordRequired.Value])
            .MinimumLength(8).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordMinLength.Value])
            .Must(password =>
            {
                if (string.IsNullOrEmpty(password)) return false;

                // At least one uppercase letter
                bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");

                // At least one lowercase letter
                bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");

                // At least one digit
                bool hasDigit = Regex.IsMatch(password, @"\d");

                // At least one special character
                bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]");

                // Only allow valid characters (letters, digits, and specific special chars)
                bool isValidFormat = Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]+$");

                return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar && isValidFormat;
            })
            .WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordValid.Value]);

            RuleFor(v => v.PasswordConfirm)
                .Equal(v => v.Password).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordConfirmNotEqual.Value]);
        }

        private async Task<bool> EmailIsNotExist(string email, CancellationToken cancellationToken)
        {
            return !await _userManager.Users.AnyAsync(x => x.Email == email, cancellationToken);
        }

        private async Task<bool> PhoneIsNotExist(string? phone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(phone)) return true;
             // Normalize phone before check if needed, or check exactly as stored
            var normalizedPhone = Regex.Replace(phone, "^0+", "");
            return !await _userManager.Users.AnyAsync(x => x.PhoneNumber == phone || x.PhoneNumber == normalizedPhone, cancellationToken);
        }

        private async Task<bool> NationalNumberIsNotExist(string nationalNumber, CancellationToken cancellationToken)
        {
             return !await _userManager.Users.AnyAsync(x => x.NationalNumber == nationalNumber, cancellationToken);
        }
    }
}
