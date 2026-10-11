using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.WebUtilities;

namespace Miltha.Tech.Core.Pagination
{
    /// <summary>
    /// Default <see cref="IUriService"/> implementation that builds page URIs from the current HTTP request.
    /// </summary>
    /// <remarks>
    /// The generated URI keeps the current scheme, host and path, and appends the
    /// <see cref="PaginationFilter.PageNumber"/> and <see cref="PaginationFilter.PageSize"/> query parameters.
    /// </remarks>
    public sealed class UriService : IUriService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        /// <summary>
        /// Initializes a new instance of the <see cref="UriService"/> class.
        /// </summary>
        /// <param name="httpContextAccessor">The accessor used to read the current request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="httpContextAccessor"/> is <see langword="null"/>.</exception>
        public UriService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor ?? throw new ArgumentNullException(nameof(httpContextAccessor));
        }

        /// <inheritdoc />
        public string GetEndpointPath(PaginationFilter paginationFilter)
        {
            ArgumentNullException.ThrowIfNull(paginationFilter);

            var request = _httpContextAccessor.HttpContext?.Request;

            if (request is null || !request.Path.HasValue)
                return null;

            var baseUri = $"{request.Scheme}://{request.Host}{request.Path}";

            var modifiedUri = QueryHelpers.AddQueryString(
                baseUri,
                nameof(PaginationFilter.PageNumber),
                paginationFilter.PageNumber.ToString());

            modifiedUri = QueryHelpers.AddQueryString(
                modifiedUri,
                nameof(PaginationFilter.PageSize),
                paginationFilter.PageSize.ToString());

            return modifiedUri;
        }
    }
}
