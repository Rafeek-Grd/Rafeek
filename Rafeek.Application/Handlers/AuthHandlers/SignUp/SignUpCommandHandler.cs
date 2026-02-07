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
using System.Text.RegularExpressions;

namespace Rafeek.Application.Handlers.AuthHandlers.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignResponse>
    {
        private readonly ISignInManager _signInManager;
        private readonly IIdentityUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IMapper _mapper;

        public SignUpCommandHandler
        (
            ISignInManager signInManager,
            IIdentityUnitOfWork ctx,
            IStringLocalizer<Messages> localizer,
            IMapper mapper
        )
        {
            _signInManager = signInManager;
            _ctx = ctx;
            _localizer = localizer;
            _mapper = mapper;
        }

        public async Task<SignResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var phone = request.Phone!.Trim();
            var userId = Guid.NewGuid();
            var user = new IdentityUser<Guid>()
            {
                Id = userId,
                UserName = $"{request.Email.Split("@")[0]}_{Guid.NewGuid().ToString("N")[..8]}".ToLowerInvariant(),
                NormalizedUserName = request.Email.ToUpperInvariant(),
                Email = request.Email,
                NormalizedEmail = request.Email.ToUpperInvariant(),
                PhoneNumber = Regex.Replace(phone, "^0+", ""),
                EmailConfirmed = true
            };

            var result = await _signInManager.SignUpAsync(user, request.Password, cancellationToken);

            if (!result.Succeeded)
            {
                throw new BadRequestException(result.Errors);
            }

            var signInResult = await _signInManager.PasswordSignInAsync(user.Email, request.Password, false, false, cancellationToken);

            if (!signInResult.Succeeded)
            {
                throw new UnauthorizedException(_localizer[LocalizationKeys.UserMessages.InvalidSignIn.Value]);
            }

            if (!string.IsNullOrWhiteSpace(request.FbToken))
            {
                var checkTokenIsAlreadyExistBefore = await _ctx.UserFbTokenRepository.GetBy(fb => fb.UserId == user.Id && fb.FbToken == request.FbToken).FirstOrDefaultAsync();
                if (checkTokenIsAlreadyExistBefore is null)
                {
                    var added = await _ctx.UserFbTokenRepository.AddAsync(new UserFbTokens() { UserId = user.Id, FbToken = request.FbToken });
                    added.CreatedBy = user.Id.ToString();
                    added.IsIosDevice = request.IsIosDevice ? true : false;
                    added.IsAndroidDevice = request.IsAndroidDevice ? true : false;
                }
                else
                {
                    checkTokenIsAlreadyExistBefore.IsDeleted = false;
                    checkTokenIsAlreadyExistBefore.IsActive = true;
                    checkTokenIsAlreadyExistBefore.IsIosDevice = request.IsIosDevice ? true : false;
                    checkTokenIsAlreadyExistBefore.IsAndroidDevice = request.IsAndroidDevice ? true : false;
                }
            }

            IdentityUser<Guid> response = (IdentityUser<Guid>)signInResult.Data;
            AuthResult tokens = (AuthResult)await _ctx.RefreshTokenRepository.GenerateTokens(response, cancellationToken);
            signInResult.Data = _mapper.Map(user, new SignResponse());
            await _ctx.SaveChangesAsync(cancellationToken);
            return _mapper.Map(tokens, (SignResponse)signInResult.Data);
        }
    }
}
