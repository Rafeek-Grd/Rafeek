using Rafeek.Application.Common.Extensions;
using Rafeek.Application.Common.Interfaces;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Rafeek.API.Services
{
    public class CurrentUserService : ICurrentUserService
    {
        public Guid UserId { get; } = Guid.Empty;
        public string? UserName { get; } = "";
        public bool IsAuthenticated { get; } = false;
        public string IpAddress { get; } = "";

        public CurrentUserService(IHttpContextAccessor httpContextAccessor, IDataEncryption dataEncryption)
        {
            try
            {
                IpAddress = httpContextAccessor.HttpContext?.Connection?.RemoteIpAddress?.ToString() ?? string.Empty;
                UserName = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);

                var encryptedId = httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(encryptedId))
                {
                    encryptedId = httpContextAccessor.HttpContext?.User?.FindFirstValue("nameid")
                                  ?? httpContextAccessor.HttpContext?.User?.FindFirstValue("sub");
                }

                if (!string.IsNullOrEmpty(encryptedId))
                {
                    if (Guid.TryParse(encryptedId, out var parsed))
                    {
                        UserId = parsed;
                    }
                    else
                    {
                        try
                        {
                            var decryptedId = dataEncryption.Decrypt(encryptedId);
                            UserId = decryptedId.ToGuid();
                        }
                        catch (CryptographicException)
                        {
                            UserId = Guid.Empty;
                        }
                        catch
                        {
                            // Any other error -> treat as unauthenticated
                            UserId = Guid.Empty;
                        }
                    }
                }
                else
                {
                    UserId = Guid.Empty;
                }

                IsAuthenticated = UserId != Guid.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

        }
    }
}
