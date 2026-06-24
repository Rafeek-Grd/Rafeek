using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static PagginatedResult<TDestination> PaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20)
        {
            List<TDestination> items;
            int count;

            if (pageNumber == -1)
            {
                items = queryable.ToList();
                count = items.Count;
            }
            else
            {
                count = queryable.Count();
                var size = pageSize <= 0 ? 20 : pageSize;
                items = queryable.Skip((pageNumber - 1) * size).Take(size).ToList();
            }

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }

        public static async Task<PagginatedResult<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20, CancellationToken cancellationToken = default)
        {
            List<TDestination> items;
            int count;

            if (pageNumber == -1)
            {
                items = await queryable.ToListAsync(cancellationToken);
                count = items.Count;
            }
            else
            {
                count = await queryable.CountAsync(cancellationToken);
                var size = pageSize <= 0 ? 20 : pageSize;
                items = await queryable.Skip((pageNumber - 1) * size).Take(size).ToListAsync(cancellationToken);
            }

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }
    }
}

