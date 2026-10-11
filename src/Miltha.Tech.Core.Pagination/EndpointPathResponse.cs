namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Represents the navigation links of a paged response.
    /// </summary>
    /// <remarks>
    /// Each property contains a URI that points to the corresponding page, or <see langword="null"/>
    /// when the page does not exist for the current result set.
    /// </remarks>
    public sealed class EndpointPathResponse
    {
        /// <summary>
        /// Gets the URI of the first page.
        /// </summary>
        public string FirstPage { get; init; }

        /// <summary>
        /// Gets the URI of the next page, or <see langword="null"/> when the current page is the last one.
        /// </summary>
        public string NextPage { get; init; }

        /// <summary>
        /// Gets the URI of the previous page, or <see langword="null"/> when the current page is the first one.
        /// </summary>
        public string PreviousPage { get; init; }

        /// <summary>
        /// Gets the URI of the last page.
        /// </summary>
        public string LastPage { get; init; }
    }
}
