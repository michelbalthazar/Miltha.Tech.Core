using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Result;
using Serilog;

namespace Miltha.Tech.Core.WebApi
{
    public static class ExceptionHandlerConfigExtensions
    {
        public static void UseExceptionHandlerConfiguration(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(c => c.Run(async context =>
            {
                var exception = context.Features
                    .Get<IExceptionHandlerPathFeature>()
                    .Error;

                Log.Fatal(exception, "Unexpected error **:");

                await context.Response.WriteAsJsonAsync(Errors.Unexpected(message: exception.Message));
            }));
        }
    }
}