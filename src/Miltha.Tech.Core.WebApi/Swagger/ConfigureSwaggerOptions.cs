using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace Miltha.Tech.Core.Api.Configuration.Swagger
{
    public static class ConfigureSwaggerOptions
    {
        public static void CreateInfoForApiVersion(this SwaggerGenOptions options, IApiVersionDescriptionProvider _provider)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
                options.SwaggerDoc(description.GroupName, CreateInfoForApiVersion(description));
        }

        private static OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = Assembly.GetEntryAssembly().GetName().Name.Replace(".", " "),
                Version = description.ApiVersion.ToString(),
                Description = "Esta API faz parte da solução Miltha Tech.",
                Contact = new OpenApiContact() { Name = "Miltha Tech", Email = "" },
            };

            if (description.IsDeprecated)
                info.Description += " Esta versão da API foi descontinuada!";

            return info;
        }
    }
}