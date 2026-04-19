namespace Rafeek.Application.Common.Interfaces
{
    public interface IAiSecurityService
    {
        bool ValidateApiKey(string apiKey);
    }
}
