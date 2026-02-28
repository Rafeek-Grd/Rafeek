using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.RefreshToken
{
    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, SignResponse>
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

        public async Task<SignResponse> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
        {
            var storedToken = await _ctx.RefreshTokenRepository.GetValidRefreshTokenAsync(request.RefreshToken, cancellationToken);

            if (storedToken is null)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.TokenMessages.NotValid.Value]);
            }

            var user = await _userManager.FindByIdAsync(storedToken.UserId);
            if (user is null)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.TokenMessages.NotValid.Value]);
            }

            _ctx.RefreshTokenRepository.Delete(storedToken);
            var tokens = (AuthResult)await _ctx.RefreshTokenRepository.GenerateTokens(user, cancellationToken);
            await _ctx.SaveChangesAsync(cancellationToken);

            var signResponse = _mapper.Map<AuthResult, SignResponse>(tokens);
            _mapper.Map(user, signResponse);

            var roles = await _userManager.GetRolesAsync(user);
            var primaryRole = roles.FirstOrDefault();
            if (!string.IsNullOrEmpty(primaryRole) && Enum.TryParse<UserType>(primaryRole, out var userType))
            {
                signResponse.Role = (int)userType;
            }

            signResponse.ProfilePictureUrl = user.ProfilePictureUrl;

            return signResponse;
        }
    }
}
