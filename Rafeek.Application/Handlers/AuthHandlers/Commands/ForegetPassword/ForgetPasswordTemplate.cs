namespace Rafeek.Application.Handlers.AuthHandlers.Commands.ForegetPassword
{
    public class ForgetPasswordTemplate
    {
        public string FullName { get; set; } = string.Empty;
        public string VerificationCode { get; set; } = string.Empty;
        public string TemplatePath { get; set; } = string.Empty;

        public ForgetPasswordTemplate(string fullName, string verificationCode, string templatePath)
        {
            FullName = fullName;
            VerificationCode = verificationCode;
            TemplatePath = templatePath;
        }
    }
}
