using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using Rafeek.Domain.Models;

namespace Rafeek.Application.Common.Interfaces
{
    public interface IEmailNotificationService
    {
        Task SendAnyTemplateToEmailByUsingEmbeddedMailKitAsync(EmailMessage emailMessage, LocalizationKeys.EmailTemplates.EmailTemplatesKeys templatesKeys, string templatePath, EmailType emailType = EmailType.Default);

    }
}
