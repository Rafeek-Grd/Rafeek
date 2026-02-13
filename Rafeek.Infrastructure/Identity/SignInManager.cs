using Microsoft.AspNetCore.Identity;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Models;
using Rafeek.Domain.Enums;
using Rafeek.Infrastructure.Extensions;
using Microsoft.EntityFrameworkCore;

using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Microsoft.Extensions.Localization;
using Rafeek.Domain.Entities;

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

        public async Task<Result> SignUpAsync(ApplicationUser user, string Password, IReadOnlyCollection<string> roles, CancellationToken cancellationToken)
        {
            if (roles == null || !roles.Any())
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.PrimayRoleInvalid]);
            }

            // Security: Deduplicate roles to prevent duplicate assignments
            var uniqueRoles = roles.Distinct(StringComparer.OrdinalIgnoreCase).ToList();

            var userByEmail = await _signInManager.UserManager.FindByEmailAsync(user.Email);
            if (userByEmail is not null)
            {
                if (userByEmail.EmailConfirmed)
                {
                    throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.EmailExist.Value]);
                }
                
                await _signInManager.UserManager.DeleteAsync(userByEmail);
            }

            if (!string.IsNullOrEmpty(user.PhoneNumber) && await _signInManager.UserManager.Users.AnyAsync(u => u.PhoneNumber == user.PhoneNumber, cancellationToken))
            {
                throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.PhoneNumberExist.Value]);
            }

            user.EmailConfirmed = true;
            var result = await _signInManager.UserManager.CreateAsync(user, Password);

            if (result.Succeeded)
            {
                // Performance: Batch check existing roles to minimize database queries
                var existingRoles = await _roleManager.Roles
                    .Where(r => uniqueRoles.Contains(r.Name))
                    .Select(r => r.Name)
                    .ToListAsync(cancellationToken);

                // Performance: Create only non-existing roles in batch
                var rolesToCreate = uniqueRoles.Except(existingRoles, StringComparer.OrdinalIgnoreCase).ToList();
                foreach (var roleToCreate in rolesToCreate)
                {
                    await _roleManager.CreateAsync(new IdentityRole<Guid>(roleToCreate));
                }

                // Security & Performance: Assign all roles to user
                foreach (var role in uniqueRoles)
                {
                    await _signInManager.UserManager.AddToRoleAsync(user, role);
                }
            }

            return result.MapToResult(result);
        }
    }
}
