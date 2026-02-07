using Microsoft.AspNetCore.Identity;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Common.Interfaces
{
    public interface ISignInManager
    {
        Task<Result> PasswordSignInAsync(string email, string password, bool isPersistent, bool LockoutOnFailure,CancellationToken cancellationToken);
        Task<Result> SignUpAsync(IdentityUser<Guid> user, string Password, CancellationToken cancellationToken);
    }
}
