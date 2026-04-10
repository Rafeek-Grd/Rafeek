using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Entities;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Microsoft.Extensions.Localization;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.CheckFromConfirmationCode
{
    public class CheckFromConfirmationCodeCommandHandler : IRequestHandler<CheckFromConfirmationCodeCommand, CheckFromConfirmationCodeResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public CheckFromConfirmationCodeCommandHandler(SignInManager<ApplicationUser> signInManager, IStringLocalizer<Messages> localizer)
        {
            _signInManager = signInManager; 
            _localizer = localizer;
        }

        public async Task<CheckFromConfirmationCodeResponse> Handle(CheckFromConfirmationCodeCommand request, CancellationToken cancellationToken)
        {
            var tokenFound = await _signInManager
                .UserManager
                .Users
                .AnyAsync(u => u.PasswordResetToken == request.ConfirmationCode
                            && u.PasswordResetTokenExpiredTime > DateTime.Now
                            && (u.Email == request.Email || u.TemporaryEmail == request.Email), cancellationToken);

            if (!tokenFound)
            {
                return new CheckFromConfirmationCodeResponse
                {
                    IsValid = false,
                    Message = _localizer[LocalizationKeys.UserMessages.ResetTokenInvalid.Value]
                };
            }

            return new CheckFromConfirmationCodeResponse
            {
                IsValid = true
            };
        }
    }
}
