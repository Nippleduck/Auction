using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Auction.Data.Pagination
{
    public class PagedCollection<T>
    {
        public PagedCollection(IEnumerable<T> items, int count, int pageNumber, int pageSize)
        {
            TotalCount = count;
            PageSize = pageSize;
            CurrentPage = pageNumber;
            TotalPages = (int)Math.Ceiling(count / (double)pageSize);
            Collection = items;
        }
        
        public IEnumerable<T> Collection { get; }
        public int CurrentPage { get; private set; }
        public int TotalPages { get; private set; }
        public int PageSize { get; private set; }
        public int TotalCount { get; private set; }

        public bool HasPrevious => CurrentPage > 1;
        public bool HasNext => CurrentPage < TotalPages;

        public PagedCollection<TDestination> TransformSource<TDestination>(Func<IEnumerable<T>, IEnumerable<TDestination>> transform) =>
            new PagedCollection<TDestination>(transform(Collection), TotalCount, CurrentPage, PageSize);
    }

    public static class PaginationExtensions
    {
        /// <summary>
        /// Paginates query depending on <paramref name="pageNumber"/> and <paramref name="pageSize"/> parameters and
        /// returns list from <paramref name="resultCallback"/> which specifies transformative method for processed query.
        /// </summary>
        /// <typeparam name="T">Type of resulting collection</typeparam>
        /// <param name="source">Source query</param>
        /// <param name="pageNumber">Specifies the amount of pages to be skipped</param>
        /// <param name="pageSize">Specifies the amount of elements to be taken</param>
        /// <param name="resultCallback">Callback which is responsible for paged query transformation to readable collection</param>
        /// <returns>Paginated collection <see cref="PagedCollection{T}"/> of type <typeparamref name="T"/></returns>
        public static async Task<PagedCollection<T>> ToPagedCollectionAsync<T>
            (this IQueryable<T> source, int pageNumber, int pageSize, Func<IQueryable<T>, Task<List<T>>> resultCallback)
            where T : class
        {
            var count = source.Count();
            var paged = source.Skip((pageNumber - 1) * pageSize).Take(pageSize);

            return new PagedCollection<T>(await resultCallback(paged), count, pageNumber, pageSize);
        }
    }
}
