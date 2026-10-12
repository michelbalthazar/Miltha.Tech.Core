using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Miltha.Tech.Core.Time
{
    public static class TimeServiceCollectionExtensions
    {
        public static IServiceCollection AddTime(
            this IServiceCollection services,
            Action<TimeZoneOptions>? configure = null)
        {
            if (configure != null)
                services.Configure(configure);

            services.AddHttpContextAccessor();

            services.TryAddScoped<ITimeZoneSource, HeaderTimeZoneSource>();
            services.TryAddScoped<ITimeZoneSource, ClaimTimeZoneSource>();
            services.TryAddScoped<ITimeZoneSource, IpGeoTimeZoneSource>();
            services.TryAddScoped<IGeoTimeZoneResolver, NullGeoTimeZoneResolver>();

            services.AddScoped<ITimeZoneProvider, TimeZoneProvider>();

            services.AddScoped<TimeProvider>(provider => new ScopedTimeProvider(provider.GetRequiredService<ITimeZoneProvider>()));

            return services;
        }
    }
}