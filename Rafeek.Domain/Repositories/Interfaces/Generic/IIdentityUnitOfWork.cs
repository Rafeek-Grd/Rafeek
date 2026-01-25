using System.Threading;
using System.Threading.Tasks;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IIdentityUnitOfWork : IAsyncDisposable
    {
        IRefreshTokenRepository RefreshTokenRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
