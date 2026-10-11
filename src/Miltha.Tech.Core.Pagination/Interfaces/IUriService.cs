namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Builds the navigation links used by <see cref="PagedResponse{T}"/>.
    /// </summary>
    public interface IUriService
    {
        /// <summary>
        /// Builds the endpoint URI for the given paging filter.
        /// </summary>
        /// <param name="paginationFilter">The filter that describes the target page.</param>
        /// <returns>The URI of the target page, or <see langword="null"/> when it cannot be resolved.</returns>
        string GetEndpointPath(PaginationFilter paginationFilter);
    }
}
