namespace Rafeek.Application.Handlers.AuthHandlers.SendUserCredentials
{
    public class SendUserCredentialsTemplate
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string TemplatePath { get; set; } = string.Empty;

        public SendUserCredentialsTemplate(string fullName, string email, string password, string templatePath)
        {
            FullName = fullName;
            Email = email;
            Password = password;
            TemplatePath = templatePath;
        }
    }
}
