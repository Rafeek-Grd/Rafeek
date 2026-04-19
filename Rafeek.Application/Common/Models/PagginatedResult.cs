namespace Rafeek.Application.Common.Models
{
    public class PagginatedResult<T>
    {
        public IReadOnlyCollection<T> Items { get; }

        public int PageNumber { get; }
        public int PageSize { get; }
        public int TotalPages { get; }
        public int TotalCount { get; }

        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;

        public PagginatedResult(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            PageNumber = pageNumber <= 0 ? 1 : pageNumber;
            PageSize = pageSize <= 0 ? 20 : pageSize;
            TotalPages = (int)Math.Ceiling(count / (double)PageSize);
            TotalCount = count;
            Items = items;
        }

        public static PagginatedResult<T> Create(IReadOnlyCollection<T> items, int count, int pageNumber, int pageSize)
        {
            return new PagginatedResult<T>(items, count, pageNumber, pageSize);
        }
    }
}
