namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IWriteRepository<T>
    {
        // add
        T Add(T entity);

        // add async
        Task<T> AddAsync(T entity, CancellationToken cancellationToken = new CancellationToken());

        // add range
        void AddRange(IEnumerable<T> entities);

        // add range async
        Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = new CancellationToken());

        // update
        void Update(T entity);

        // update range
        void UpdateRange(IEnumerable<T> entities);

        // remove
        void Delete(T entity);

        // remove range
        void DeleteRange(IEnumerable<T> entities);
    }
}
