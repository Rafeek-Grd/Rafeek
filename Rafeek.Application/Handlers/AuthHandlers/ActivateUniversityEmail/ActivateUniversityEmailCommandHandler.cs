using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Handlers.AuthHandlers.ActivateUniversityEmail
{
    public class ActivateUniversityEmailCommandHandler : IRequestHandler<ActivateUniversityEmailCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IStringLocalizer<Messages> _localizer;

        public ActivateUniversityEmailCommandHandler(
            UserManager<ApplicationUser> userManager,
            IStringLocalizer<Messages> localizer)
        {
            _userManager = userManager;
            _localizer = localizer;
        }

        public async Task<string> Handle(ActivateUniversityEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (user is null)
            {
                throw new NotFoundException(_localizer[LocalizationKeys.UserMessages.NotFound.Value]);
            }

            if (user.IsUniversityEmailActivated)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.EmailAlreadyActivated.Value]);
            }

            user.IsUniversityEmailActivated = true;
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.FailedToActivateEmail.Value]);
            }

            return _localizer[LocalizationKeys.UserMessages.EmailActivatedSuccessfully.Value];
        }
    }
}
