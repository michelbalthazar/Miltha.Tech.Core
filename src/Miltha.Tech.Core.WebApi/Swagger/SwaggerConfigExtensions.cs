using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace Miltha.Tech.Core.Api.Configuration.Swagger
{
    public static class SwaggerConfigExtensions
    {
        public static IServiceCollection AddSwaggerConfiguration(this IServiceCollection services, string xmlCustomUISwaggerPath)
        {
            services.AddSwaggerGen(c =>
            {
                c.CreateInfoForApiVersion(services.BuildServiceProvider().GetRequiredService<IApiVersionDescriptionProvider>());

                c.OperationFilter<SwaggerDefaultValues>();

                c.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Description = "Enter your authorization token as follows: `Bearer Token-Gerado`",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                            Reference = new OpenApiReference
                            {
                                Id = "Bearer",
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new List<string>()
                    }
                });

                c.SchemaFilter<EnumSchemaFilter>();

                if (File.Exists(xmlCustomUISwaggerPath))
                    c.IncludeXmlComments(xmlCustomUISwaggerPath);
            });

            return services;
        }

        public static IApplicationBuilder UseSwaggerConfiguration(this IApplicationBuilder app)
        {
            app.UseSwagger(c => c.SerializeAsV2 = true);
            app.UseSwaggerUI(c =>
            {
                foreach (var description in app.ApplicationServices.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
                    c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
            });

            return app;
        }
    }
}