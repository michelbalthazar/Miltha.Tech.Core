using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Miltha.Tech.Core.Time
{
    public sealed class ClaimTimeZoneSource : ITimeZoneSource
    {
        private readonly TimeZoneOptions _options;

        public ClaimTimeZoneSource(IOptions<TimeZoneOptions> options)
        {
            _options = options.Value;
        }

        public int Order => 200;

        public TimeZoneInfo? Resolve(HttpContext context)
        {
            var zoneId = context.User.FindFirst(_options.ClaimType)?.Value;

            if (string.IsNullOrWhiteSpace(zoneId))
                return null;

            return TimeZoneHelpers.TryGetTimeZone(zoneId, out var timeZone) ? timeZone : null;
        }
    }
}