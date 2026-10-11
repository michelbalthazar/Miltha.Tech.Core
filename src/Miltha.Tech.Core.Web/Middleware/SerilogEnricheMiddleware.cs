using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Http;
using Serilog.Context;

namespace Miltha.Tech.Core.Web.Middleware
{
    public class SerilogEnricheMiddleware
    {
        private const string UserIdClaimType = "UserId";
        private const string UserNameClaimType = "UserName";
        private readonly RequestDelegate _next;

        public SerilogEnricheMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            using (LogContext.PushProperty("UserId", GetUserId(context)))
            using (LogContext.PushProperty("UserName", GetUserName(context)))
            using (LogContext.PushProperty("CorrelationId", GetCorrelationId(context)))
            {
                return _next.Invoke(context);
            }
        }

        private static Guid GetUserId(HttpContext context)
        {
            var id = context.User.FindFirst(UserIdClaimType)?.Value;

            return string.IsNullOrWhiteSpace(id) || !Guid.TryParse(id, out var userId) ? Guid.Empty : userId;
        }

        private static string GetUserName(HttpContext context)
        {
            var userName = context.User.FindFirst(UserNameClaimType)?.Value;

            return string.IsNullOrWhiteSpace(userName) ? string.Empty : userName;
        }

        private static string GetCorrelationId(HttpContext context) => context.Request.Headers.GetCorrelationId() ?? string.Empty;
    }
}