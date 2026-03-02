using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;

namespace Rafeek.Infrastructure.Notifications.Emails
{
    public interface IEmailNotificationService
    {
        Task SendConfirmationCodeToEmailByUsingEmbeddedMailKitAsync(EmailMessage emailMessage, LocalizationKeys.EmailTemplates.EmailTemplatesKeys templatesKeys, string templatePath, EmailType emailType = EmailType.Default);
    }
}
