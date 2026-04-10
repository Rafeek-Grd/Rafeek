using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, ResetPasswordResponse>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public ResetPasswordCommandHandler(
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<Messages> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<ResetPasswordResponse> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == request.Email || u.TemporaryEmail == request.Email);

            var removePasswordResult = await _userManager.RemovePasswordAsync(user);
            if (!removePasswordResult.Succeeded)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.PasswordResetSuccess.Value]);
            }

            var addPasswordResult = await _userManager.AddPasswordAsync(user, request.NewPassword);
            if (!addPasswordResult.Succeeded)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.PasswordResetSuccess.Value]);
            }

            user.PasswordResetToken = null;
            user.PasswordResetTokenExpiredTime = null;
            await _userManager.UpdateAsync(user);

            return new ResetPasswordResponse
            {
                Message = _localizer[LocalizationKeys.UserMessages.PasswordResetSuccess.Value]
            };
        }
    }
}
