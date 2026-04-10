using Microsoft.EntityFrameworkCore;
using Rafeek.Application.Common.Models;

namespace Rafeek.Application.Common.Mappings
{
    public static class MappingExtensions
    {
        public static PagginatedResult<TDestination> PaginatedList<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20)
        {
            var count = queryable.Count();
            var items = queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }

        public static async Task<PagginatedResult<TDestination>> PaginatedListAsync<TDestination>(this IQueryable<TDestination> queryable, int pageNumber = 1, int pageSize = 20)
        {
            var count = await queryable.CountAsync();
            var items = await queryable.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            return PagginatedResult<TDestination>.Create(items, count, pageNumber, pageSize);
        }
    }
}

