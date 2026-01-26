using Rafeek.Application.Common.Models;
using Rafeek.Domain.Entities;

namespace Rafeek.Application.Common.Interfaces
{
    public interface ISignInManager
    {
        Task<Result> PasswordSignInAsync(string email, string password, bool isPersistent, bool LockoutOnFailure,CancellationToken cancellationToken);
        Task<Result> SignUpAsync(ApplicationUser user, string Password, CancellationToken cancellationToken);
    }
}
