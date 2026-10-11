using Microsoft.Extensions.DependencyInjection;

namespace Miltha.Tech.Core.Api.Configuration
{
    public static class AutoMapperConfig
    {
        public static void RegisterMappings(this IServiceCollection services)
        {
            services.AddAutoMapper(_ => { }, AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
