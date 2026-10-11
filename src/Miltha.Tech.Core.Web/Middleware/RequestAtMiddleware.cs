using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Http;

namespace Miltha.Tech.Core.Web.Middleware
{
    public class RequestAtMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestAtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(PropagationKeyHelper.RequestAtKey, out var requestAt))
            {
                requestAt = DateTimeOffset.UtcNow.ToString("O");
                context.Request.Headers[PropagationKeyHelper.RequestAtKey] = requestAt;
            }

            context.Items[PropagationKeyHelper.RequestAtKey] = requestAt.ToString();

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[PropagationKeyHelper.RequestAtKey] = requestAt.ToString();
                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}