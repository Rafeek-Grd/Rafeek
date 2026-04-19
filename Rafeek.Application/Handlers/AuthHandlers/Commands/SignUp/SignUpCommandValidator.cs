using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using System.Text.RegularExpressions;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpCommandValidator(IStringLocalizer<Messages> localizer, UserManager<ApplicationUser> userManager)
        {
            _localizer = localizer;
            _userManager = userManager;

            RuleFor(v => v.PrimaryRole)
                .Must(r => Enum.IsDefined(typeof(UserType), r))
                .WithMessage(_localizer[LocalizationKeys.UserMessages.PrimayRoleInvalid.Value]);

            RuleFor(v => v.AdditionalRoles)
                .Must(roles => roles == null || roles.All(r => Enum.IsDefined(typeof(UserType), r)))
                .WithMessage(_localizer[LocalizationKeys.UserMessages.AdditionalRolesInvalid.Value]);

            RuleFor(v => v)
                .Must(command => 
                {
                    var allRoles = new HashSet<UserType> { command.PrimaryRole };
                    if (command.AdditionalRoles != null)
                    {
                        foreach (var r in command.AdditionalRoles) allRoles.Add(r);
                    }

                    // 1. Single Role Only Case
                    if (allRoles.Count == 1) return true;

                    // 2. Multi-Role Constraints
                    // Students, Staff, and Instructors cannot have any other roles
                    if (allRoles.Contains(UserType.Student) || 
                        allRoles.Contains(UserType.Staff) || 
                        allRoles.Contains(UserType.Instructor))
                    {
                        return false;
                    }

                    // Admin and SubAdmin can ONLY be combined with Doctor
                    // They cannot be combined with each other
                    if (allRoles.Contains(UserType.Admin) && allRoles.Contains(UserType.SubAdmin)) return false;

                    if (allRoles.Contains(UserType.Admin) || allRoles.Contains(UserType.SubAdmin))
                    {
                        // If Admin/SubAdmin is present in a multi-role set, the ONLY other allowed role is Doctor
                        return allRoles.All(r => r == UserType.Admin || r == UserType.SubAdmin || r == UserType.Doctor);
                    }

                    return false; // Any other multi-role combination is invalid
                })
                .WithMessage(_localizer[LocalizationKeys.UserMessages.PrimayRoleInvalid.Value]);

            RuleFor(v => v.FullName)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().NotNull().WithMessage(_localizer[LocalizationKeys.UserMessages.FullNameRequired.Value]);

            RuleFor(v => v.Phone)
                .Cascade(CascadeMode.Stop)
                .NotEmpty().NotNull().WithMessage(_localizer[LocalizationKeys.UserMessages.PhoneRequired.Value])
                .MaximumLength(20).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneInvalid.Value])
                .Matches(@"^\s*0?\d+\s*$").WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneInvalid.Value])
                .MustAsync(PhoneIsNotExist).WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneNumberExist.Value]);

            RuleFor(v => v.TemporaryEmail)
                .Cascade(CascadeMode.Stop)
                .NotNull().NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.EmailRequired.Value])
                .EmailAddress().WithMessage(_localizer[LocalizationKeys.GlobalValidationMessages.EmailInvalid.Value])
                .MustAsync(TemporaryEmailIsNotExist).WithMessage(_localizer[LocalizationKeys.UserMessages.EmailAlreadyExistedbefore.Value]);

            RuleFor(v => v.NationalNumber)
                .NotEmpty().WithMessage(_localizer[LocalizationKeys.UserMessages.NationalNumberRequired.Value]);

             RuleFor(x => x.Gender)
                .Must(g => !g.HasValue || Enum.IsDefined(typeof(GenderType), g.Value))
                .WithMessage(_localizer[LocalizationKeys.UserMessages.GenderIsNotValid.Value]);

            RuleFor(v => v.Password)
                .Cascade(CascadeMode.Stop)
                .MinimumLength(8).WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordMinLength.Value])
                .Must(password =>
                {
                    if (string.IsNullOrEmpty(password)) return false;

                    bool hasUpperCase = Regex.IsMatch(password, @"[A-Z]");
                    bool hasLowerCase = Regex.IsMatch(password, @"[a-z]");
                    bool hasDigit = Regex.IsMatch(password, @"\d");
                    bool hasSpecialChar = Regex.IsMatch(password, @"[!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]");
                    bool isValidFormat = Regex.IsMatch(password, @"^[a-zA-Z0-9!@#$%^&*()_+=\-{}\[\]:;""'|\\<>,.?/~`]+$");

                    return hasUpperCase && hasLowerCase && hasDigit && hasSpecialChar && isValidFormat;
                })
                .WithMessage(_localizer[LocalizationKeys.UserMessages.PasswordValid.Value])
                .When(x => !string.IsNullOrWhiteSpace(x.Password));
        }

        private async Task<bool> TemporaryEmailIsNotExist(string temporaryEmail, CancellationToken cancellationToken)
        {
            return !await _userManager.Users.AnyAsync(x => x.TemporaryEmail == temporaryEmail, cancellationToken);
        }

        private async Task<bool> PhoneIsNotExist(string? phone, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(phone)) return true;
            var normalizedPhone = Regex.Replace(phone, "^0+", "");
            return !await _userManager.Users.AnyAsync(x => x.PhoneNumber == phone || x.PhoneNumber == normalizedPhone, cancellationToken);
        }
    }
}
