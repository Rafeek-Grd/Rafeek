using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.ResetPassword
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
            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
            {
                throw new NotFoundException(_localizer[LocalizationKeys.UserMessages.NotFound.Value]);
            }

            var result = await _userManager.ResetPasswordAsync(user, request.Token, request.NewPassword);

            if (!result.Succeeded)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.ResetTokenInvalid.Value]);
            }

            return new ResetPasswordResponse
            {
                Message = _localizer[LocalizationKeys.UserMessages.PasswordResetSuccess.Value]
            };
        }
    }
}
