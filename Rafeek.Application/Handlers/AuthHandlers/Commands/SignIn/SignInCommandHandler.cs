using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Handlers.AuthHandlers.Commands;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Application.Handlers.AuthHandlers.Commands.SignIn
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, SignResponse>
    {
        private readonly ISignInManager _signInManager;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IMapper _mapper;
        private readonly IRafeekDbContext _dbContext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ICurrentUserService _currentUserService;

        public SignInCommandHandler(
            ISignInManager signInManager,
            IUnitOfWork unitOfWork,
            IStringLocalizer<Messages> localizer,
            IMapper mapper,
            UserManager<ApplicationUser> userManager,
            ICurrentUserService currentUserService,
            IRafeekDbContext dbContext)
        {
            _signInManager = signInManager;
            _unitOfWork = unitOfWork;
            _localizer = localizer;
            _mapper = mapper;
            _dbContext = dbContext;
            _userManager = userManager;
            _currentUserService = currentUserService;
        }

        public async Task<SignResponse> Handle(SignInCommand request, CancellationToken cancellationToken)
        {
            // Find user by university email or temporary email
            var currentUser = await _userManager.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email || u.TemporaryEmail == request.Email, cancellationToken);
            
            if (currentUser is null)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.InvalidSignIn.Value]);
            }

            var result = await _signInManager.PasswordSignInAsync(currentUser.UserName!, request.Password, false, false, cancellationToken);


            if (!result.Succeeded)
            {
                var signInResult = (SignInResult)result.MainResult!;

                if (signInResult.IsLockedOut)
                {
                    throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.Locked.Value]);
                }

                if (signInResult.IsNotAllowed)
                {
                    throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.EmailUnVerified.Value]);
                }

                throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.InvalidSignIn.Value]);
            }

            if (!string.IsNullOrWhiteSpace(request.FbToken))
            {
                await HandleFirebaseTokenAsync(currentUser.Id, request.FbToken, request.IsIosDevice, request.IsAndroidDevice, cancellationToken);
            }

            var loginHistory = new UserLoginHistory
            {
                UserId = currentUser.Id,
                LoginTime = DateTime.UtcNow,
                IpAddress = _currentUserService.IpAddress
            };
            await _dbContext.UserLoginHistories.AddAsync(loginHistory);

            var authResult = (AuthResult)await _unitOfWork.RefreshTokenRepository.GenerateTokens(currentUser, cancellationToken);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            var signResponse = _mapper.Map<AuthResult, SignResponse>(authResult);
            _mapper.Map(currentUser, signResponse);

            var roles = await _userManager.GetRolesAsync(currentUser);
            signResponse.Roles = roles.ToList();

            return signResponse;
        }

        private async Task HandleFirebaseTokenAsync(
            Guid userId,
            string fbToken,
            bool isIosDevice,
            bool isAndroidDevice,
            CancellationToken cancellationToken)
        {
            var existingToken = await _unitOfWork.UserFbTokenRepository
                .GetBy(fb => fb.FbToken == fbToken && fb.UserId == userId)
                .FirstOrDefaultAsync(cancellationToken);

            if (existingToken is null)
            {
                await _unitOfWork.UserFbTokenRepository.AddAsync(new UserFbTokens
                {
                    UserId = userId,
                    FbToken = fbToken,
                    IsIosDevice = isIosDevice,
                    IsAndroidDevice = isAndroidDevice
                });
            }
            else
            {
                existingToken.IsDeleted = false;
                existingToken.IsActive = true;
                existingToken.IsIosDevice = isIosDevice;
                existingToken.IsAndroidDevice = isAndroidDevice;
            }
        }
    }
}