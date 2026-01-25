namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IUnitOfWork: IAsyncDisposable
    {
        IRefreshTokenRepository RefreshTokenRepository { get; }

        Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
