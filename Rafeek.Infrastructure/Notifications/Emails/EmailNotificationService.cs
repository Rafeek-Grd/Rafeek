using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using Rafeek.Application.Common.Options;
using Rafeek.Application.Localization;
using Rafeek.Domain.Enums;
using System.Dynamic;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;

namespace Rafeek.Infrastructure.Notifications.Emails
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IOptions<EmailConfiguration> _emailConfig;

        public EmailNotificationService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig;
        }

        public async Task SendConfirmationCodeToEmailByUsingEmbeddedMailKitAsync(
            EmailMessage emailMessage,
            LocalizationKeys.EmailTemplates.EmailTemplatesKeys templatesKeys,
            string templatePath,
            EmailType emailType = EmailType.Default)
        {
            try
            {
                var settings = _emailConfig.Value.EmailSettings?.FirstOrDefault(x => x.EmailType == emailType)
                              ?? _emailConfig.Value.EmailSettings?.FirstOrDefault(x => x.EmailType == EmailType.Default);

                if (settings == null)
                    throw new InvalidOperationException("SMTP settings not found for this email type.");

                string templateContent;
                using (var stream = GetType().Assembly.GetManifestResourceStream(templatePath))
                {
                    if (stream == null)
                        throw new InvalidOperationException($"Email template resource not found: {templatePath}");

                    using (var reader = new StreamReader(stream))
                    {
                        templateContent = await reader.ReadToEndAsync();
                    }
                }

                string bodyHtml = ReplaceTemplateTokens(templateContent, emailMessage.Body);

                // Create the email
                var email = new MimeMessage();
                email.From.Add(new MailboxAddress(settings.Email, settings.Email));
                email.To.Add(new MailboxAddress(emailMessage.To, emailMessage.To));
                email.Subject = emailMessage.Subject ?? templatesKeys.Subject.Value;

                var bodyBuilder = new BodyBuilder
                {
                    HtmlBody = bodyHtml
                };
                email.Body = bodyBuilder.ToMessageBody();

                using (var smtp = new MailKit.Net.Smtp.SmtpClient())
                {
                    await smtp.ConnectAsync(settings.Host, settings.Port, SecureSocketOptions.StartTls);
                    smtp.AuthenticationMechanisms.Remove("XOAUTH2");
                    await smtp.AuthenticateAsync(settings.Email, settings.Password);
                    await smtp.SendAsync(email);
                    await smtp.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email notification.", ex);
            }
        }

        private string ReplaceTemplateTokens(string template, object model)
        {
            var dict = ToExpando(model);

            foreach (var kvp in dict)
            {
                string token = $"@(Model.{kvp.Key})";
                string value = kvp.Value?.ToString() ?? string.Empty;
                template = template.Replace(token, value, StringComparison.Ordinal);
            }

            return template;
        }

        private static ExpandoObject ToExpando(object model)
        {
            if (model is ExpandoObject exp)
            {
                return exp;
            }

            IDictionary<string, object> expando = new ExpandoObject();
            foreach (var propertyDescriptor in model.GetType().GetTypeInfo().GetProperties())
            {
                var obj = propertyDescriptor.GetValue(model);

                if (obj != null && IsAnonymousType(obj.GetType()))
                {
                    obj = ToExpando(obj);
                }

                expando.Add(propertyDescriptor.Name, obj);
            }

            return (ExpandoObject)expando;
        }

        private static bool IsAnonymousType(Type type)
        {
            bool hasCompilerGeneratedAttribute = type.GetTypeInfo()
                .GetCustomAttributes(typeof(CompilerGeneratedAttribute), false)
                .Any();

            bool nameContainsAnonymousType = type.FullName.Contains("AnonymousType");
            bool isAnonymousType = hasCompilerGeneratedAttribute && nameContainsAnonymousType;

            return isAnonymousType;
        }
    }
}