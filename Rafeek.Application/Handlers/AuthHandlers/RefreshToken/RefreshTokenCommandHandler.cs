using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, AuthResult>
    {
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public RefreshTokenCommandHandler(
            IUnitOfWork ctx,
            IStringLocalizer<Messages> localizer,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            _ctx = ctx;
            _localizer = localizer;
            _mapper = mapper;
            _userManager = userManager;
        }

        public async Task<AuthResult> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var refreshToken = await _ctx.RefreshTokenRepository.GetValidRefreshTokenAsync(request.Token, cancellationToken);
            if (refreshToken is null) throw new UnauthorizedException(_localizer[LocalizationKeys.TokenMessages.NotValid.Value]);
            if (refreshToken.IsExpired || !refreshToken.IsActive) throw new UnauthorizedException(_localizer[LocalizationKeys.TokenMessages.Expired.Value]);

            var user = await _userManager.FindByIdAsync(refreshToken.UserId);
            if (user is null) throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.NotFound.Value]);
            if (user.LockoutEnabled && user.LockoutEnd > DateTime.UtcNow) throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.Locked.Value]);

            refreshToken.Revoke();
            await _ctx.SaveChangesAsync(cancellationToken);

            var newRefreshTokens = (AuthResult)await _ctx.RefreshTokenRepository.GenerateTokens(user, cancellationToken);
            return newRefreshTokens;
        }
    }
}
