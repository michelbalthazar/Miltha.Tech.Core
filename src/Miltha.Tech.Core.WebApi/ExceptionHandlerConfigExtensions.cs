using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Miltha.Tech.Core.Http;
using Miltha.Tech.Core.Result;
using Serilog;

namespace Miltha.Tech.Core.WebApi
{
    public static class ExceptionHandlerConfigExtensions
    {
        /// <summary>
        /// Registers the global exception handler: captures every unhandled exception across the
        /// pipeline and returns the standard <see cref="ApiErrorResponse"/> contract
        /// (500 / application/problem+json) without leaking internal details to the caller.
        /// The full exception is logged.
        /// </summary>
        public static void UseExceptionHandlerConfiguration(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder => builder.Run(async context =>
            {
                var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

                Log.Fatal(exception, "Unexpected error **:");

                if (context.Response.HasStarted)
                    return;

                var traceId = context.Request.Headers.GetCorrelationId();

                if (string.IsNullOrWhiteSpace(traceId))
                    traceId = context.TraceIdentifier;

                var response = ApiErrorResponse.From(Errors.Unexpected(), context.Request.Path, traceId);

                context.Response.Clear();
                context.Response.StatusCode = response.Status;
                context.Response.ContentType = ApiErrorResponse.ContentType;

                await context.Response.WriteAsJsonAsync(response, options: null, contentType: ApiErrorResponse.ContentType);
            }));
        }
    }
}