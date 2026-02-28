using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, SignResponse>
    {
        private readonly ISignInManager _signInManager;
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IMapper _mapper;
        private readonly IRafeekDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public SignInCommandHandler(
            ISignInManager signInManager,
            IUnitOfWork ctx,
            IStringLocalizer<Messages> localizer,
            IMapper mapper,
            IRafeekDbContext dbContext,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService)
        {
            _signInManager = signInManager;
            _ctx = ctx;
            _localizer = localizer;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<SignResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(
                request.Email, request.Password, false, false, cancellationToken);

            if (!signInResult.Succeeded)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.InvalidSignIn.Value]);
            }

            var user = (ApplicationUser)signInResult.Data!;

            if (!string.IsNullOrWhiteSpace(request.FbToken))
            {
                var existingToken = await _ctx.UserFbTokenRepository
                    .GetBy(fb => fb.UserId == user.Id && fb.FbToken == request.FbToken)
                    .FirstOrDefaultAsync(cancellationToken);

                if (existingToken is null)
                {
                    await _ctx.UserFbTokenRepository.AddAsync(new UserFbTokens
                    {
                        UserId = user.Id,
                        FbToken = request.FbToken,
                        IsIosDevice = request.IsIosDevice,
                        IsAndroidDevice = request.IsAndroidDevice,
                        CreatedBy = user.Id.ToString()
                    });
                }
                else
                {
                    existingToken.IsDeleted = false;
                    existingToken.IsActive = true;
                    existingToken.IsIosDevice = request.IsIosDevice;
                    existingToken.IsAndroidDevice = request.IsAndroidDevice;
                }
            }

            var loginHistory = new UserLoginHistory
            {
                UserId = user.Id,
                LoginTime = DateTime.UtcNow,
                IpAddress = _currentUserService.IpAddress,
                CreatedBy = user.Id.ToString()
            };
            await _dbContext.UserLoginHistories.AddAsync(loginHistory, cancellationToken);

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
