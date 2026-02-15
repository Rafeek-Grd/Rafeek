using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IUnitOfWork : IAsyncDisposable
    {
        IRefreshTokenRepository RefreshTokenRepository { get; }
        IUserFbTokenRepository UserFbTokenRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);
        IExecutionStrategy CreateExecutionStrategy();
    }
}
