using Rafeek.Domain.Entities;
using Rafeek.Domain.Repositories.Interfaces.Generic;

namespace Rafeek.Domain.Repositories.Interfaces
{
    public interface IUserRepository: IGenericRepository<ApplicationUser, Guid>
    {
        Task SendConfirmationCodeAsync(string email, CancellationToken cancellationToken);
    }
}
