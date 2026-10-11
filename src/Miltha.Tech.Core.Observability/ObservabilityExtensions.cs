using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Exceptions;
using Serilog.Sinks.SystemConsole.Themes;

namespace Miltha.Tech.Core.Observability
{
    /// <summary>
    /// Centralizes Serilog configuration and injection for applications using the Miltha.Tech.Core.
    /// </summary>
    public static class ObservabilityExtensions
    {
        /// <summary>
        /// Configures Serilog (enrichers, Seq and Console sinks) and registers it
        /// as a singleton <see cref="ILogger"/>, also setting the static <see cref="Log.Logger"/>.
        /// </summary>
        /// <param name="services">The service collection to register into.</param>
        /// <param name="configuration">The application configuration used to read the `Logging:Serilog:Url` setting.</param>
        /// <returns>The <paramref name="services"/> instance, for chaining.</returns>
        public static IServiceCollection AddObservability(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ILogger>(_ =>
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
    }
}