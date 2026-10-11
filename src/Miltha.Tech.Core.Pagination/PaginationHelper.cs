namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Provides helpers to build <see cref="PagedResponse{T}"/> instances from a page of data and a total record count.
    /// </summary>
    public static class PaginationHelper
    {
        /// <summary>
        /// Creates a <see cref="PagedResponse{T}"/> for the given page of data.
        /// </summary>
        /// <typeparam name="T">The type of the paged data.</typeparam>
        /// <param name="pagedData">The data of the current page.</param>
        /// <param name="recordsCount">The total number of records available across all pages.</param>
        /// <param name="pagination">The filter that produced the current page.</param>
        /// <param name="uriService">The service used to build the navigation links. When <see langword="null"/>, the links are left empty.</param>
        /// <returns>A <see cref="PagedResponse{T}"/> with the data and its paging metadata.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="pagedData"/> or <paramref name="pagination"/> is <see langword="null"/>.</exception>
        public static PagedResponse<IEnumerable<T>> CreatePagedResponse<T>(
            IEnumerable<T> pagedData,
            long recordsCount,
            PaginationFilter pagination,
            IUriService uriService = null)
        {
            ArgumentNullException.ThrowIfNull(pagedData);
            ArgumentNullException.ThrowIfNull(pagination);

            var pages = CalculateTotalPages(recordsCount, pagination.PageSize);

            var endpointPath = BuildEndpointPath(pagination, pages, uriService);

            return new PagedResponse<IEnumerable<T>>(pagedData, pagination, endpointPath, recordsCount, pages);
        }

        /// <summary>
        /// Calculates the total number of pages for the given record count and page size.
        /// </summary>
        /// <param name="totalRecords">The total number of records.</param>
        /// <param name="pageSize">The page size.</param>
        /// <returns>The total number of pages, or 0 when there are no records or the page size is not positive.</returns>
        public static int CalculateTotalPages(long totalRecords, int pageSize)
        {
            if (totalRecords <= 0 || pageSize <= 0)
                return 0;

            return (int)Math.Ceiling(totalRecords / (double)pageSize);
        }

        private static EndpointPathResponse BuildEndpointPath(PaginationFilter pagination, int pages, IUriService uriService)
        {
            if (uriService is null || pages == 0)
                return new EndpointPathResponse();

            return new EndpointPathResponse
            {
                FirstPage = uriService.GetEndpointPath(new PaginationFilter(PaginationFilter.DefaultPageNumber, pagination.PageSize)),
                PreviousPage = pagination.PageNumber > PaginationFilter.DefaultPageNumber
                    ? uriService.GetEndpointPath(new PaginationFilter(pagination.PageNumber - 1, pagination.PageSize))
                    : null,
                NextPage = pagination.PageNumber < pages
                    ? uriService.GetEndpointPath(new PaginationFilter(pagination.PageNumber + 1, pagination.PageSize))
                    : null,
                LastPage = uriService.GetEndpointPath(new PaginationFilter(pages, pagination.PageSize))
            };
        }
    }
}
