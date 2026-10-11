using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mime;
using System.Text.Json;

namespace Miltha.Tech.Core.WebApi.HealthCheck
{
    public static class HealthCheckConfigExtensions
    {
        public static void AddHealthCheckConfiguration(this IServiceCollection services)
        {
            services.AddHealthChecks();
        }

        public static void UseHealthCheckConfiguration(this IApplicationBuilder app)
        {
            app.UseHealthChecks("/health-check",
                 new HealthCheckOptions()
                 {
                     ResponseWriter = async (context, report) =>
                     {
                         var result = JsonSerializer.Serialize(
                            new
                            {
                                currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
                                statusApplication = report.Status.ToString(),
                            });

                         context.Response.ContentType = MediaTypeNames.Application.Json;
                         await context.Response.WriteAsync(result);
                     }
                 });
        }
    }
}