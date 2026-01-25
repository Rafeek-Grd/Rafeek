using System.Linq.Expressions;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IEntityRepository<T, TKey> : IReadRepository<T>, IWriteRepository<T> where T : class
    {
        // Exists by key
        bool ExistsByKey(TKey key);

        // Exists by key async
        Task<bool> ExistsByKeyAsync(TKey key, CancellationToken cancellationToken = default);

        // Find by key
        T? FindByKey(TKey key);

        // Find by key async
        Task<T?> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default);

        // Include all navigational properties
        IQueryable<T> IncludeAll();

        // Get first including all navigational properties
        Task<T?> GetFirstIncludingAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);

        // Get single including all navigational properties
        Task<T?> GetSingleIncludingAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default);
    }
}
