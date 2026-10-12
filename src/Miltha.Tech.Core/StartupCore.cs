using System.Reflection;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Miltha.Tech.Core.Observability;
using Miltha.Tech.Core.Security;
using Miltha.Tech.Core.WebApi;

namespace Miltha.Tech.Core
{
    public abstract class StartupCore : StartupBase
    {
        public IConfiguration Configuration { get; private set; }

        protected void BuildConfiguration<T>(IHostEnvironment hostEnvironment) where T : class
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location))
                .AddJsonFile("appsettings.json", true, true)
                .AddJsonFile($"appsettings.{hostEnvironment.EnvironmentName}.json", true, true)
                .AddEnvironmentVariables();

            if (hostEnvironment.IsDevelopment())
            {
                builder.AddUserSecrets<T>();
            }

            Configuration = builder.Build();
        }

        protected void ConfigureDefaultServices(IServiceCollection services)
        {
            services.AddApiConfiguration();

            services.AddObservability(Configuration);

            services.AddJwtConfiguration(Configuration);

            services.AddOpenApiConfiguration();
        }

        protected static void UseServices(IApplicationBuilder app)
        {
            app.UseApiConfiguration();
        }
    }
}