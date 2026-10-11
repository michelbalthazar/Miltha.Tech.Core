using Microsoft.AspNetCore.Builder;
using Miltha.Tech.Core.Web.Middleware;

namespace Miltha.Tech.Core.WebApi
{
    /// <summary>
    /// Configures the authentication/authorization pipeline used by Miltha.Tech APIs.
    /// </summary>
    public static class AuthConfigExtensions
    {
        /// <summary>
        /// Enables JWT authentication, tenant resolution and authorization in the request pipeline.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>The <paramref name="app"/> instance, for chaining.</returns>
        public static IApplicationBuilder UseAuthConfiguration(this IApplicationBuilder app)
        {
            app.UseAuthentication();
            app.UseMiddleware<TenantResolutionMiddleware>();
            app.UseAuthorization();

            return app;
        }
    }
}