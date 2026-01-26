using System.Linq.Expressions;

namespace Rafeek.Domain.Repositories.Interfaces.Generic
{
    public interface IReadRepository<T>
    {
        // getall 
        IQueryable<T> GetAll();

        // getall with filter
        IQueryable<T> GetAll(Expression<Func<T, bool>> predicate);

        // get single
        T GetSingle(Func<T, bool> predicate);

        // get single async
        Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken());

        // get by filter
        IQueryable<T> GetBy(Expression<Func<T, bool>> predicate);

        // get first 
        T GetFirst(Func<T, bool> predicate);

        // get first async
        Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken());

        // exists
        bool Exists(Func<T, bool> predicate);

        // exists async
        Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = new CancellationToken());
    }
}
