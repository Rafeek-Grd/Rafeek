using Rafeek.Domain.Enums;

namespace Rafeek.Infrastructure.Notifications.Emails
{
    public class EmailConfiguration
    {
        public string Email { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
        public List<EmailSettings> EmailSettings { get; set; } = null!;
    }

    public class EmailSettings
    {
        public EmailType EmailType { get; set; }
        public string Email { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string Password { get; set; } = string.Empty;
        public bool EnableSsl { get; set; }
    }
}
