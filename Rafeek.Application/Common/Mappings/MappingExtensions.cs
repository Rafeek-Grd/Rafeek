using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static PagginatedResult<TDestination> PaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20)
        {
            var count = queryable.Count();
            
            List<TDestination> items;
            if (pageSize == -1)
            {
                items = queryable.ToList();
            }
            else
            {
                var size = pageSize <= 0 ? 20 : pageSize;
                items = queryable.Skip((pageNumber - 1) * size).Take(size).ToList();
            }

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }

        public static async Task<PagginatedResult<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            var count = await queryable.CountAsync(cancellationToken);
            
            List<TDestination> items;
            if (pageSize == -1)
            {
                items = await queryable.ToListAsync(cancellationToken);
            }
            else
            {
                var size = pageSize <= 0 ? 20 : pageSize;
                items = await queryable.Skip((pageNumber - 1) * size).Take(size).ToListAsync(cancellationToken);
            }

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }
    }
}

