using Microsoft.AspNetCore.Identity;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Microsoft.Extensions.Localization;

namespace Rafeek.Infrastructure.Identity
{
    public class SignInManager : ISignInManager
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole<Guid>> _roleManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public SignInManager(SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole<Guid>> roleManager, IStringLocalizer<Messages> localizer)
        {
            _signInManager = signInManager;
            _roleManager = roleManager;
            _localizer = localizer;
        }

        public async Task<Result> PasswordSignInAsync(string email, string password, bool isPersistent, bool LockoutOnFailure, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _signInManager.UserManager.FindByEmailAsync(email);
                var result = await _signInManager.CheckPasswordSignInAsync(user!, password, LockoutOnFailure);
                return result.MapToResult(user!);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<Result> SignUpAsync(ApplicationUser user, string Password, CancellationToken cancellationToken)
        {
            var userByEmail = await _signInManager.UserManager.FindByEmailAsync(user.Email);
            if (userByEmail is not null)
            {
                if (userByEmail.EmailConfirmed)
                {
                    throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.EmailExist.Value]);
                }
                
                await _signInManager.UserManager.DeleteAsync(userByEmail);
            }

            if (await _signInManager.UserManager.Users.AnyAsync(u => u.NationalNumber == user.NationalNumber, cancellationToken))
            {
                throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.NationalNumberExist.Value]);
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber) && await _signInManager.UserManager.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber, cancellationToken))
            {
                throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneNumberExist.Value]);
            }

            if (!string.IsNullOrEmpty(user.Code) && await _signInManager.UserManager.Users.AnyAsync(u => u.Code == user.Code, cancellationToken))
            {
                throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.UserCodeExist.Value]);
            }

            user.EmailConfirmed = true;
            var result = await _signInManager.UserManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                var roleName = ((UserType)user.UserType).ToString();

                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(roleName));
                }

                await _signInManager.UserManager.AddToRoleAsync(user, roleName);
            }

            return result.MapToResult(result);
        }
    }
}
