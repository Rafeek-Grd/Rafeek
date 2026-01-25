using Microsoft.EntityFrameworkCore;
using Rafeek.Domain.Repositories.Interfaces.Generic;
using Rafeek.Persistence;
using System.Linq.Expressions;

namespace Rafeek.Infrastructure.Repostiories.Implementations.Generic
{
    public class EntityRepository<T, TKey> : IEntityRepository<T, TKey> where T : class
    {
        protected readonly RafeekDbContext _dbContext;
        protected readonly DbSet<T> _dbSet;

        public EntityRepository(RafeekDbContext dbContext)
        {
            _dbContext = dbContext;
            _dbSet = _dbContext.Set<T>();
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

        public async Task<bool> ExistsAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            return _dbSet.AsEnumerable().Any(predicate);
        }

        public async Task<bool> ExistsByKeyAsync(TKey key, CancellationToken cancellationToken = default)
        {
            var entity = await _dbSet.FindAsync(key , cancellationToken);
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
            return await _dbSet.FindAsync(key , cancellationToken);
        }

        public IQueryable<T> GetAll()
        {
            return _dbSet.AsQueryable();
        }

        public IQueryable<T> GetAll(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public IQueryable<T> GetBy(Func<T, bool> predicate)
        {
            return _dbSet.Where(predicate).AsQueryable();
        }

        public T GetFirst(Func<T, bool> predicate)
        {
            return _dbSet.First(predicate);
        }

        public async Task<T> GetFirstAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
            return await Task.Run(() => _dbSet.First(predicate), cancellationToken);
        }

        public T GetSingle(Func<T, bool> predicate)
        {
            return _dbSet.Single(predicate);
        }

        public async Task<T> GetSingleAsync(Func<T, bool> predicate, CancellationToken cancellationToken = default)
        {
             return await Task.Run(() => _dbSet.Single(predicate), cancellationToken);
        }

        public void Update(T entity)
        {
            _dbSet.Update(entity);
        }

        public void UpdateRange(IEnumerable<T> entities)
        {
            _dbSet.UpdateRange(entities);
        }

        public IQueryable<T> IncludeAll()
        {
            var query = _dbSet.AsQueryable();
            var navigations = _dbContext.Model.FindEntityType(typeof(T))
                .GetNavigations();

            foreach (var property in navigations)
            {
                query = query.Include(property.Name);
            }

            return query;
        }

        public async Task<T?> GetFirstIncludingAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await IncludeAll().FirstOrDefaultAsync(predicate, cancellationToken);
        }

        public async Task<T?> GetSingleIncludingAllAsync(Expression<Func<T, bool>> predicate, CancellationToken cancellationToken = default)
        {
            return await IncludeAll().SingleOrDefaultAsync(predicate, cancellationToken);
        }
    }
}
