using Asp.Versioning;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Miltha.Tech.Core.Http;
using Miltha.Tech.Core.Pagination;
using Miltha.Tech.Core.Providers;
using Miltha.Tech.Core.Providers.Interfaces;
using Miltha.Tech.Core.Web.Middleware;
using Miltha.Tech.Core.WebApi.Converters;
using Miltha.Tech.Core.WebApi.Cors;
using Miltha.Tech.Core.WebApi.Handlers;
using Miltha.Tech.Core.WebApi.HealthCheck;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Miltha.Tech.Core.WebApi
{
    public static class ApiConfigurationExtensions
    {
        public static IServiceCollection AddApiConfiguration(this IServiceCollection services)
        {
            services
            .AddApiVersioning(options =>
            {
                options.ReportApiVersions = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(1, 0);
            })
            .AddApiExplorer(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
                options.AssumeDefaultVersionWhenUnspecified = true;
            });

            services.AddResponseCompression(options =>
            {
                options.EnableForHttps = true;
                options.Providers.Add<GzipCompressionProvider>();
            });

            services.AddControllers(options =>
            {
                options.RespectBrowserAcceptHeader = true;
                options.ModelMetadataDetailsProviders.Add(new SystemTextJsonValidationMetadataProvider());
            })
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
                options.JsonSerializerOptions.Converters.Add(new DateTimeConverter());
            });

            services.AddCorsConfiguration();

            services.AddHealthCheckConfiguration();

            services.AddScoped<ITenantProvider, HttpContextTenantProvider>();

            services.AddScoped<IUriService, UriService>();

            services.AddHttpContextAccessor();

            services.AddMemoryCache();

            services.AddTransient<CorrelationIdPropagationHandler>();

            services.AddTransient<RequestAtPropagationHandler>();

            services.AddHttpClient<IHttpClienteInternal, HttpClienteInternal>()
                .AddHttpMessageHandler<CorrelationIdPropagationHandler>()
                .AddHttpMessageHandler<RequestAtPropagationHandler>();

            return services;
        }

        public static IServiceCollection AddLogging(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILogger>(sp =>
            {
                return Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Information()
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .Enrich.WithExceptionStackTraceHash()
                .Enrich.WithEnvironmentName()
                .Enrich.WithMachineName()
                .Enrich.WithAssemblyName()
                .Enrich.WithAssemblyVersion()
                .WriteTo.Seq(configuration["Logging:Serilog:Url"])
                .WriteTo.Console(theme: AnsiConsoleTheme.Sixteen)
                .CreateLogger();
            });

            return services;
        }

        public static IApplicationBuilder UseApiConfiguration(this IApplicationBuilder app)
        {
            app.UseResponseCompression();

            app.UseHttpsRedirection();

            app.UseExceptionHandlerConfiguration();

            app.UseMiddleware<CorrelationIdMiddleware>();

            app.UseMiddleware<CultureMiddleware>();

            app.UseMiddleware<RequestAtMiddleware>();

            app.UseMiddleware<SerilogEnricheMiddleware>();

            app.UseStaticFiles();

            app.UseCorsConfiguration();

            app.UseHealthCheckConfiguration();

            app.UseRouting();

            app.UseAuthConfiguration();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            return app;
        }
    }
}