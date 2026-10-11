using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Providers.Interfaces;

namespace Miltha.Tech.Core.Web.Middleware
{
    public sealed class TenantResolutionMiddleware
    {
        private const string TenantClaimType = "tenant_id";
        private readonly RequestDelegate _next;

        public TenantResolutionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context, ITenantProvider tenantProvider)
        {
            var claim = context.User.FindFirst(TenantClaimType);

            if (claim != null && Guid.TryParse(claim.Value, out var tenantId))
                tenantProvider.SetTenant(tenantId);

            if (context.User.Identity?.IsAuthenticated == true && !tenantProvider.HasTenant)
            {
                context.Response.StatusCode = StatusCodes.Status403Forbidden;
                return;
            }

            await _next(context);
        }
    }
}