namespace Rafeek.Application.Common.Models
{
    public class PagginatedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; }

        public int PageNumber { get; } = 1;
        public int PageSize { get; } = 20;

        public int TotalPages { get; }

        public int TotalCount { get; }

        public bool HasPreviousPage => PageNumber > 1;

        public bool HasNextPage => PageNumber < TotalPages;

        public PagginatedResult(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            TotalCount = count;
            Items = items;
        }

        public static PagginatedResult<T> Create(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            return new PagginatedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
