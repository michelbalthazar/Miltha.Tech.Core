using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Extensions.Helper;
using Miltha.Tech.Core.Helper;

namespace Miltha.Tech.Core.WebApi.Handlers
{
    public class RequestAtPropagationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var requestAt = httpContextAccessor.HttpContext?.Request.Headers.GetRequestAt();

            if (!string.IsNullOrWhiteSpace(requestAt) && !request.Headers.Contains(PropagationKeyHelper.RequestAtKey))
                request.Headers.TryAddWithoutValidation(PropagationKeyHelper.RequestAtKey, requestAt);

            return base.SendAsync(request, cancellationToken);
        }
    }
}