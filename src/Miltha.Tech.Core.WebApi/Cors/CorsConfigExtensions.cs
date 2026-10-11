using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Miltha.Tech.Core.WebApi.Cors
{
    public static class CorsConfigExtensions
    {
        public static void AddCorsConfiguration(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll",
                    builder =>
                    {
                        builder.AllowAnyOrigin();
                        builder.AllowAnyMethod();
                        builder.AllowAnyHeader();
                    });
            });
        }

        public static void UseCorsConfiguration(this IApplicationBuilder app)
        {
            app.UseCors("AllowAll");
        }
    }
}