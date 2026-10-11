using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi;

namespace Miltha.Tech.Core.WebApi
{
    /// <summary>
    /// Configures the native .NET OpenAPI document generation used by Miltha.Tech APIs,
    /// replacing the previous Swagger/Swashbuckle setup.
    /// </summary>
    public static class OpenApiConfigExtensions
    {
        /// <summary>
        /// Registers OpenAPI document generation (endpoint at <c>/openapi/v1.json</c>) with standard
        /// document metadata and a Bearer security scheme so the generated specification reflects
        /// the JWT authentication adopted by the platform.
        /// </summary>
        /// <param name="services">The service collection to register into.</param>
        /// <returns>The <paramref name="services"/> instance, for chaining.</returns>
        public static IServiceCollection AddOpenApiConfiguration(this IServiceCollection services)
        {
            services.AddOpenApi(options =>
            {
                options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_1;

                options.AddDocumentTransformer((document, _, _) =>
                {
                    document.Info = new OpenApiInfo
                    {
                        Title = "Miltha.Tech API",
                        Description = "Esta API faz parte da solução Miltha Tech.",
                        Version = "v1",
                        Contact = new OpenApiContact { Name = "Miltha Tech" }
                    };

                    document.Components ??= new OpenApiComponents();
                    document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();

                    document.Components.SecuritySchemes["Bearer"] = new OpenApiSecurityScheme
                    {
                        Name = "Authorization",
                        Description = "Informe o token de acesso como: `Bearer Token-Gerado`",
                        In = ParameterLocation.Header,
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer"
                    };

                    document.Security ??= new List<OpenApiSecurityRequirement>();
                    document.Security.Clear();
                    document.Security.Add(new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document, null)] = new List<string>()
                    });

                    return Task.CompletedTask;
                });
            });

            return services;
        }
    }
}