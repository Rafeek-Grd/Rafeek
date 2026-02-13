using AutoMapper;
using Microsoft.Extensions.Logging;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Rafeek.Application.Common.Exceptions;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace Rafeek.Application.Handlers.AuthHandlers.SignUp
{
    public class SignUpCommandHandler : IRequestHandler<SignUpCommand, SignResponse>
    {
        private readonly ISignInManager _signInManager;
        private readonly IUnitOfWork _ctx;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IMapper _mapper;
        private readonly IRafeekDbContext _dbContext;
        private readonly ILogger<SignUpCommandHandler> _logger;

        public SignUpCommandHandler
        (
            ISignInManager signInManager,
            IUnitOfWork ctx,
            IStringLocalizer<Messages> localizer,
            IMapper mapper,
            IRafeekDbContext dbContext,
            ILogger<SignUpCommandHandler> logger
        )
        {
            _signInManager = signInManager;
            _ctx = ctx;
            _localizer = localizer;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<SignResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var strategy = _ctx.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _ctx.BeginTransactionAsync(cancellationToken);
                try
                {
                    var phone = request.Phone!.Trim();
                    var userId = Guid.NewGuid();
                    var user = new ApplicationUser()
                    {
                        Id = userId,
                        UserName = $"{request.Email.Split("@")[0]}_{Guid.NewGuid().ToString("N")[..8]}".ToLowerInvariant(),
                        NormalizedUserName = request.Email.ToUpperInvariant(),
                        Email = request.Email,
                        NormalizedEmail = request.Email.ToUpperInvariant(),
                        FullName = request.FullName,
                        NationalId = request.NationalNumber,
                        PhoneNumber = Regex.Replace(phone, "^0+", ""),
                        EmailConfirmed = true
                    };

                    var primaryRole = request.PrimaryRole;
                    
                    var allRoles = new List<string> { primaryRole.ToString() };
                    if (request.AdditionalRoles?.Any() == true)
                    {
                        allRoles.AddRange(request.AdditionalRoles.Select(r => r.ToString()));
                    }

                    // Create user with all roles
                    var result = await _signInManager.SignUpAsync(user, request.Password, allRoles, cancellationToken);

                    if (!result.Succeeded)
                    {
                        throw new BadRequestException(_localizer[LocalizationKeys.UserMessages.FailedSignUp.Value]);
                    }

                    if (primaryRole == UserType.Student)
                    {
                        // Security & Performance: Generate unique 9-digit university code
                        string universityCode = await GenerateUniqueUniversityCodeAsync(cancellationToken);
                        
                        var student = new Student
                        {
                            UserId = user.Id,
                            UniversityCode = universityCode,
                            Status = StudentStatus.Active,
                            Level = 1,
                            Term = 1,
                            AcademicProfile = new StudentAcademicProfile
                            {
                                GPA = 0,
                                CGPA = 0,
                                CompletedCredits = 0,
                                RemainingCredits = 0,
                                Standing = Standing.Freshman.ToString()
                            }
                        };

                        await _dbContext.Students.AddAsync(student, cancellationToken);
                    }
                    else if (primaryRole == UserType.Instructor)
                    {
                        var instructor = new Instructor
                        {
                            UserId = user.Id
                        };
                        await _dbContext.Instructors.AddAsync(instructor, cancellationToken);
                    }
                    else if (primaryRole == UserType.Doctor)
                    {
                        var doctor = new Doctor
                        {
                            UserId = user.Id
                        };
                        await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
                    }

                    await _ctx.SaveChangesAsync(cancellationToken);

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
                            added.IsIosDevice = request.IsIosDevice;
                            added.IsAndroidDevice = request.IsAndroidDevice;
                        }
                        else
                        {
                            checkTokenIsAlreadyExistBefore.IsDeleted = false;
                            checkTokenIsAlreadyExistBefore.IsActive = true;
                            checkTokenIsAlreadyExistBefore.IsIosDevice = request.IsIosDevice;
                            checkTokenIsAlreadyExistBefore.IsAndroidDevice = request.IsAndroidDevice;
                        }
                    }

                    ApplicationUser response = (ApplicationUser)signInResult.Data;
                    AuthResult tokens = (AuthResult)await _ctx.RefreshTokenRepository.GenerateTokens(response, cancellationToken);
                    
                    await _ctx.SaveChangesAsync(cancellationToken);
                    await transaction.CommitAsync(cancellationToken);

                    var signResponse = _mapper.Map(tokens, new SignResponse());
                    _mapper.Map(user, signResponse);
                    
                    signResponse.Role = (int)primaryRole;
                    
                    return signResponse;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during sign up for user {Email}", request.Email);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        private async Task<string> GenerateUniqueUniversityCodeAsync(CancellationToken cancellationToken)
        {
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var randomNumber = RandomNumberGenerator.GetInt32(100000000, 1000000000);
                var universityCode = randomNumber.ToString();

                // Check if this code is already used
                var exists = await _dbContext.Students
                    .AnyAsync(s => s.UniversityCode == universityCode, cancellationToken);

                if (!exists)
                {
                    return universityCode;
                }

                attempts++;
            }

            throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.UniversityCodeMultipleAttemps.Value]);
        }
    }
}
