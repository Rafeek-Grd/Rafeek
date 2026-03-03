using Microsoft.AspNetCore.Routing.Template;

namespace Rafeek.Application.Common.Options
{
    public class TemplatePath
    {
        public string BaseTemplatePath { get; set; } = string.Empty;
        public Templates Templates { get; set; } = null!;
    }

    public class Templates
    {
        public string ForgetPassword { get; set; } = string.Empty;
        public string SendUserCredentials { get; set; } = string.Empty;
    }
}
