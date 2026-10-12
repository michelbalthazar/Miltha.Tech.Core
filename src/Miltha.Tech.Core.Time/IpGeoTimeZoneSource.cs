using Microsoft.AspNetCore.Http;

namespace Miltha.Tech.Core.Time
{
    public sealed class IpGeoTimeZoneSource : ITimeZoneSource
    {
        private readonly IGeoTimeZoneResolver _geoTimeZoneResolver;

        public IpGeoTimeZoneSource(IGeoTimeZoneResolver geoTimeZoneResolver)
        {
            _geoTimeZoneResolver = geoTimeZoneResolver;
        }

        public int Order => 300;

        public TimeZoneInfo? Resolve(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress?.ToString();

            if (string.IsNullOrWhiteSpace(ip))
                return null;

            return _geoTimeZoneResolver.Resolve(ip);
        }
    }
}