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
using Microsoft.AspNetCore.Identity;
using Rafeek.Application.Handlers.AuthHandlers.SendUserCredentials;

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
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMediator _mediator;

        public SignUpCommandHandler
        (
            ISignInManager signInManager,
            IUnitOfWork ctx,
            IStringLocalizer<Messages> localizer,
            IMapper mapper,
            IRafeekDbContext dbContext,
            ILogger<SignUpCommandHandler> logger,
            UserManager<ApplicationUser> userManager,
            IMediator mediator
        )
        {
            _signInManager = signInManager;
            _ctx = ctx;
            _localizer = localizer;
            _mapper = mapper;
            _dbContext = dbContext;
            _logger = logger;
            _userManager = userManager;
            _mediator = mediator;
        }

        public async Task<SignResponse> Handle(SignUpCommand request, CancellationToken cancellationToken)
        {
            var strategy = _ctx.CreateExecutionStrategy();

            return await strategy.ExecuteAsync(async () =>
            {
                using var transaction = await _ctx.BeginTransactionAsync(cancellationToken);
                try
                {
                    if (string.IsNullOrWhiteSpace(request.Password))
                    {
                        var generated = GenerateSecurePassword();
                        request.Password = generated;
                    }

                    var phone = request.Phone!.Trim();
                    var userId = Guid.NewGuid();
                    
                    var universityEmail = await GenerateUniqueUniversityEmailAsync(request.FullName, request.PrimaryRole, cancellationToken);
                    
                    var user = new ApplicationUser()
                    {
                        Id = userId,
                        UserName = $"{universityEmail.Split("@")[0]}_{Guid.NewGuid().ToString("N")[..8]}".ToLowerInvariant(),
                        NormalizedUserName = universityEmail.ToUpperInvariant(),
                        Email = universityEmail,
                        TemporaryEmail = request.TemporaryEmail,
                        NormalizedEmail = universityEmail.ToUpperInvariant(),
                        ProfilePictureUrl = request.ImageName,
                        FullName = request.FullName,
                        NationalId = request.NationalNumber,
                        PhoneNumber = Regex.Replace(phone, "^0+", ""),
                        EmailConfirmed = true,
                        IsUniversityEmailActivated = false
                    };

                    var primaryRole = request.PrimaryRole;

                    var allRoles = new List<string> { primaryRole.ToString() };
                    if (request.AdditionalRoles != null && request.AdditionalRoles.Any())
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
                        string employeeCode = await GenerateUniqueEmployeeCodeAsync(cancellationToken);
                        var instructor = new Instructor
                        {
                            UserId = user.Id,
                            EmployeeCode = employeeCode
                        };
                        await _dbContext.Instructors.AddAsync(instructor, cancellationToken);
                    }
                    else if (primaryRole == UserType.Doctor)
                    {
                        string employeeCode = await GenerateUniqueEmployeeCodeAsync(cancellationToken);
                        var doctor = new Doctor
                        {
                            UserId = user.Id,
                            EmployeeCode = employeeCode,
                            IsAcademicAdvisor = request.IsAcademicAdvisor
                        };
                        await _dbContext.Doctors.AddAsync(doctor, cancellationToken);
                    }
                    else if (primaryRole == UserType.Staff)
                    {
                        string employeeCode = await GenerateUniqueEmployeeCodeAsync(cancellationToken);
                        var staff = new Staff
                        {
                            UserId = user.Id,
                            EmployeeCode = employeeCode
                        };
                        await _dbContext.Staffs.AddAsync(staff, cancellationToken);
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

                    try
                    {
                        _logger.LogInformation("Attempting to send credentials email to newly created user: {Email}", user.Email);
                        await _mediator.Send(new SendUserCredentialsCommand() { Email = user.Email, Password = request.Password }, cancellationToken);
                    }
                    catch (Exception emailEx)
                    {
                        _logger.LogError(emailEx, "Failed to send credentials email to {Email}, but user was created successfully", user.Email);
                    }

                    var signResponse = _mapper.Map(tokens, new SignResponse());
                    _mapper.Map(user, signResponse);

                    signResponse.Roles = allRoles;

                    return signResponse;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error occurred during sign up for temporary email {TemporaryEmail}", request.TemporaryEmail);
                    await transaction.RollbackAsync(cancellationToken);
                    throw;
                }
            });
        }

        private async Task<string> GenerateUniqueUniversityEmailAsync(string fullName, UserType userType, CancellationToken cancellationToken)
        {
            var normalizedName = Regex.Replace(fullName.Trim().ToLowerInvariant(), @"[^a-z\s]", "");
            var nameParts = normalizedName.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            
            string baseEmail;
            if (nameParts.Length >= 2)
            {
                baseEmail = $"{nameParts[0]}.{nameParts[^1]}";
            }
            else
            {
                baseEmail = nameParts[0];
            }

            string domain = userType == UserType.Student ? "@std.mans.edu.eg" : "@mans.edu.eg";
            string email = baseEmail + domain;

            var exists = await _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);
            
            if (!exists)
            {
                return email;
            }

            int counter = 1;
            const int maxAttempts = 100;
            
            while (counter < maxAttempts)
            {
                email = $"{baseEmail}{counter}{domain}";
                exists = await _userManager.Users.AnyAsync(u => u.Email == email, cancellationToken);
                
                if (!exists)
                {
                    return email;
                }
                
                counter++;
            }

            throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.UniversityEmailMultipleAttemps.Value]);
        }

        private async Task<string> GenerateUniqueUniversityCodeAsync(CancellationToken cancellationToken)
        {
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var randomNumber = RandomNumberGenerator.GetInt32(100000000, 1000000000);
                var universityCode = randomNumber.ToString();

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

        private async Task<string> GenerateUniqueEmployeeCodeAsync(CancellationToken cancellationToken)
        {
            const int maxAttempts = 10;
            int attempts = 0;

            while (attempts < maxAttempts)
            {
                var randomNumber = RandomNumberGenerator.GetInt32(100000000, 1000000000);
                var employeeCode = randomNumber.ToString();

                var existsInStaff = await _dbContext.Staffs
                    .AnyAsync(s => s.EmployeeCode == employeeCode, cancellationToken);
                var existsInDoctors = await _dbContext.Doctors
                    .AnyAsync(d => d.EmployeeCode == employeeCode, cancellationToken);
                var existsInInstructors = await _dbContext.Instructors
                    .AnyAsync(i => i.EmployeeCode == employeeCode, cancellationToken);

                if (!existsInStaff && !existsInDoctors && !existsInInstructors)
                {
                    return employeeCode;
                }

                attempts++;
            }

            throw new BadRequestException(_localizer[LocalizationKeys.GlobalValidationMessages.UniversityCodeMultipleAttemps.Value]);
        }

        private string GenerateSecurePassword(int length = 12)
        {
            const string upper = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lower = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string special = "!@$?_-";

            var all = upper + lower + digits + special;

            if (length < 8) length = 8;

            var password = new char[length];

            password[0] = upper[RandomNumberGenerator.GetInt32(upper.Length)];
            password[1] = lower[RandomNumberGenerator.GetInt32(lower.Length)];
            password[2] = digits[RandomNumberGenerator.GetInt32(digits.Length)];
            password[3] = special[RandomNumberGenerator.GetInt32(special.Length)];

            for (int i = 4; i < length; i++)
            {
                password[i] = all[RandomNumberGenerator.GetInt32(all.Length)];
            }

            for (int i = password.Length - 1; i > 0; i--)
            {
                int j = RandomNumberGenerator.GetInt32(i + 1);
                var tmp = password[i];
                password[i] = password[j];
                password[j] = tmp;
            }

            return new string(password);
        }
    }
}
