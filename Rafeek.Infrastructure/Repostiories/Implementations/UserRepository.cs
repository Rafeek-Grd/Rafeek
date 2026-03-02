using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using Rafeek.Application.Common.Interfaces;
using Rafeek.Application.Common.Options;
using Rafeek.Application.Handlers.AuthHandlers.ForegetPassword;
using Rafeek.Application.Localization;
using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Infrastructure.Notifications.Emails;
using Rafeek.Infrastructure.Repostiories.Implementations.Generic;
using System.Security.Cryptography;

namespace Rafeek.Infrastructure.Repostiories.Implementations
{
    public class UserRepository : BaseEntityRepository<ApplicationUser, Guid>, IUserRepository
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IOptions<TemplatePath> _options;
        private readonly IRafeekIdentityDbContext _context;
        private readonly IStringLocalizer<Messages> _localizer;
        private readonly IEmailNotificationService _emailNotificationService;

        public UserRepository(
            IRafeekIdentityDbContext context,
            SignInManager<ApplicationUser> signInManager,
            IOptions<TemplatePath> options,
            IStringLocalizer<Messages> localizer,
            IEmailNotificationService emailNotificationService
        ) : base(context)
        {
            _signInManager = signInManager;
            _options = options;
            _localizer = localizer;
            _emailNotificationService = emailNotificationService;
        }

        public async Task SendConfirmationCodeAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _signInManager.UserManager.Users.FirstOrDefaultAsync(u => u.Email == email, cancellationToken);
            if (user == null) return;

            var tokenCode = Convert.ToString(RandomNumberGenerator.GetInt32(100000, 1000000));
            user.PasswordResetToken = tokenCode;
            user.PasswordResetTokenExpiredTime = DateTime.Now.AddMinutes(5);
            
            await _signInManager.UserManager.UpdateAsync(user);
            
            var ForgetPasswordTemplate = new ForgetPasswordTemplate(user.FullName, tokenCode, _options.Value.BaseTemplatePath + _options.Value.Templates.ForgetPassword);
            var emailMessage = new EmailMessage
            {
                To = user?.Email!,
                Body = ForgetPasswordTemplate,
                Subject = _localizer[LocalizationKeys.EmailTemplates.ForgotPassword.Subject.Value]
            };

            await _emailNotificationService.SendConfirmationCodeToEmailByUsingEmbeddedMailKitAsync(emailMessage, LocalizationKeys.EmailTemplates.ForgotPassword, ForgetPasswordTemplate.TemplatePath, Domain.Enums.EmailType.OTP);
            return;
        }
    }
}
