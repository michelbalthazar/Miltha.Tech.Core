namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Represents a paging request (page number and page size) applied to a query.
    /// </summary>
    /// <remarks>
    /// Values are normalized on construction: <see cref="PageNumber"/> is never lower than
    /// <see cref="DefaultPageNumber"/> and <see cref="PageSize"/> is always between
    /// <see cref="DefaultPageSize"/> and <see cref="MaxPageSize"/>.
    /// </remarks>
    public sealed class PaginationFilter
    {
        /// <summary>
        /// The page number used when the requested value is not valid.
        /// </summary>
        public const int DefaultPageNumber = 1;

        /// <summary>
        /// The page size used when the requested value is not valid.
        /// </summary>
        public const int DefaultPageSize = 30;

        /// <summary>
        /// The maximum page size allowed by the filter.
        /// </summary>
        public const int MaxPageSize = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class using the default page number and size.
        /// </summary>
        public PaginationFilter()
            : this(DefaultPageNumber, DefaultPageSize)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PaginationFilter"/> class.
        /// </summary>
        /// <param name="pageNumber">The requested page number. Values lower than <see cref="DefaultPageNumber"/> are normalized to <see cref="DefaultPageNumber"/>.</param>
        /// <param name="pageSize">The requested page size. Values lower than 1 use <see cref="DefaultPageSize"/> and values greater than <see cref="MaxPageSize"/> are normalized to <see cref="MaxPageSize"/>.</param>
        public PaginationFilter(int pageNumber, int pageSize)
        {
            PageNumber = pageNumber < DefaultPageNumber ? DefaultPageNumber : pageNumber;
            PageSize = pageSize < 1 ? DefaultPageSize : Math.Min(pageSize, MaxPageSize);
        }

        /// <summary>
        /// Gets the normalized page number.
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        /// Gets the normalized page size.
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        /// Gets the number of records to skip for the current page.
        /// </summary>
        public int Skip => (PageNumber - 1) * PageSize;

        /// <summary>
        /// Gets the number of records to take for the current page.
        /// </summary>
        public int Take => PageSize;

        /// <summary>
        /// Creates a filter using the default page number and page size.
        /// </summary>
        /// <returns>A <see cref="PaginationFilter"/> with <see cref="DefaultPageNumber"/> and <see cref="DefaultPageSize"/>.</returns>
        public static PaginationFilter Default() => new(DefaultPageNumber, DefaultPageSize);
    }
}