namespace Rafeek.Infrastructure.Notifications.Emails
{
    public class EmailMessage
    {
        public string To { get; set; } = string.Empty;
        public string Subject { get; set; } = string.Empty;
        public object Body { get; set; } = null!;
    }
}
