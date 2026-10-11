using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Http;

namespace Miltha.Tech.Core.WebApi.Handlers
{
    public class CorrelationIdPropagationHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var correlationId = httpContextAccessor.HttpContext?.Request.Headers.GetCorrelationId();

            if (!string.IsNullOrWhiteSpace(correlationId) && !request.Headers.Contains(PropagationKeyHelper.CorrelationKey))
                request.Headers.TryAddWithoutValidation(PropagationKeyHelper.CorrelationKey, correlationId);

            return base.SendAsync(request, cancellationToken);
        }
    }
}