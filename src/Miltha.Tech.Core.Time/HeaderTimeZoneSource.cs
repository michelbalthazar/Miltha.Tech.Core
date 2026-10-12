using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Miltha.Tech.Core.Time
{
    public sealed class HeaderTimeZoneSource : ITimeZoneSource
    {
        private readonly TimeZoneOptions _options;

        public HeaderTimeZoneSource(IOptions<TimeZoneOptions> options)
        {
            _options = options.Value;
        }

        public int Order => 100;

        public TimeZoneInfo? Resolve(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(_options.HeaderName, out var value))
                return null;

            return TimeZoneHelpers.TryGetTimeZone(value.ToString(), out var timeZone) ? timeZone : null;
        }
    }
}