namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Represents a page of results together with its paging metadata.
    /// </summary>
    /// <typeparam name="T">The type of the paged data.</typeparam>
    public sealed class PagedResponse<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PagedResponse{T}"/> class.
        /// </summary>
        /// <param name="data">The data of the current page.</param>
        /// <param name="paginationFilter">The filter that produced the current page.</param>
        /// <param name="endpointPath">The navigation links of the page.</param>
        /// <param name="totalRecords">The total number of records available across all pages.</param>
        /// <param name="pages">The total number of pages.</param>
        /// <exception cref="ArgumentNullException"><paramref name="paginationFilter"/> is <see langword="null"/>.</exception>
        public PagedResponse(T data, PaginationFilter paginationFilter, EndpointPathResponse endpointPath, long totalRecords, int pages)
        {
            ArgumentNullException.ThrowIfNull(paginationFilter);

            Data = data;
            Number = paginationFilter.PageNumber;
            Size = paginationFilter.PageSize;
            TotalRecords = totalRecords;
            Pages = pages;
            EndpointPath = endpointPath ?? new EndpointPathResponse();
        }

        /// <summary>
        /// Gets the data of the current page.
        /// </summary>
        public T Data { get; }

        /// <summary>
        /// Gets the current page number.
        /// </summary>
        public int Number { get; }

        /// <summary>
        /// Gets the current page size.
        /// </summary>
        public int Size { get; }

        /// <summary>
        /// Gets the total number of records available across all pages.
        /// </summary>
        public long TotalRecords { get; }

        /// <summary>
        /// Gets the total number of pages.
        /// </summary>
        public int Pages { get; }

        /// <summary>
        /// Gets the navigation links of the page.
        /// </summary>
        public EndpointPathResponse EndpointPath { get; }
    }
}
