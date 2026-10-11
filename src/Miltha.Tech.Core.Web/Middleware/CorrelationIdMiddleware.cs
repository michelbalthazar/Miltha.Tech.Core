using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Helper;

namespace Miltha.Tech.Core.Web.Middleware
{
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(PropagationKeyHelper.CorrelationKey, out var correlationId))
            {
                correlationId = Guid.NewGuid().ToString();
                context.Request.Headers[PropagationKeyHelper.CorrelationKey] = correlationId;
            }

            context.Items[PropagationKeyHelper.CorrelationKey] = correlationId;

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[PropagationKeyHelper.CorrelationKey] = correlationId;
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}