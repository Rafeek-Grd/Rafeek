using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Persistence;
using System.Linq.Expressions;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class EntityRepository<T, TKey> : IEntityRepository<T, TKey> where T : class
    {
        protected readonly DbContext _context;
        protected readonly DbSet<T> _dbSet;

        public EntityRepository(DbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public T Add(T entity)
        {
            return _dbSet.Add(entity).Entity;
        }

        public async Task<T> AddAsync(T entity, CancellationToken cancellationToken = default)
        {
            var entry = await _dbSet.AddAsync(entity, cancellationToken);
            return entry.Entity;
        }

        public void AddRange(IEnumerable<T> entities)
        {
            _dbSet.AddRange(entities);
        }

        public async Task AddRangeAsync(IEnumerable<T> entities, CancellationToken cancellationToken = default)
        {
            await _dbSet.AddRangeAsync(entities, cancellationToken);
        }

        public void Delete(T entity)
        {
            _dbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public bool Exists(Func<T, bool> predicate)
        {
            return _dbSet.Any(predicate);
        }

        public async Task<bool> ExistsAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.AsQueryable().AnyAsync(predicate, cancellationToken);
        }

        public async Task<bool> ExistsByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(new object?[] { key }, cancellationToken);
            return entity != null;
        }

        public bool ExistsByKey(TKey key)
        {
             var entity = _dbSet.Find(key);
             return entity != null;
        }

        public T? FindByKey(TKey key)
        {
            return _dbSet.Find(key);
        }

        public async Task<T?> FindByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FindAsync(new object?[] { key }, cancellationToken);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetAll(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public IQueryable<T> GetBy(Expression<Func<T, bool>> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public T GetFirst(Func<T, bool> predicate)
        {
            return _dbSet.First(predicate);
        }

        public async Task<T> GetFirstAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.FirstAsync(predicate, cancellationToken);
        }

        public T GetSingle(Func<T, bool> predicate)
        {
            return _dbSet.Single(predicate);
        }

        public async Task<T> GetSingleAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await _dbSet.SingleAsync(predicate, cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public IQueryable<T> IncludeAll(Expression<Func<T, bool>>? predicate)
        {
            var query = GetAll(predicate!);
            var entityType = _context.Model.FindEntityType(typeof(T));

            if (entityType == null)
                return query;

            var navigations = entityType.GetNavigations();

            foreach (var property in navigations)
            {
                query = query.Include(property.Name);
            }

            return query;
        }

        public IQueryable<T> GetFirstIncludingAll(Expression<Func<T, bool>> predicate)
        {
            return IncludeAll(null).Where(predicate);
        }
    }
}
