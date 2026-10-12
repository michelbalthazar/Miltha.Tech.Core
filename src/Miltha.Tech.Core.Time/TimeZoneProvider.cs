using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Miltha.Tech.Core.Time
{
    public sealed class TimeZoneProvider : ITimeZoneProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly TimeZoneOptions _options;
        private readonly IEnumerable<ITimeZoneSource> _sources;

        public TimeZoneProvider(IHttpContextAccessor httpContextAccessor, IEnumerable<ITimeZoneSource> sources, IOptions<TimeZoneOptions> options)
        {
            _httpContextAccessor = httpContextAccessor;
            _sources = sources;
            _options = options.Value;
        }

        public TimeZoneInfo GetTimeZone()
        {
            var context = _httpContextAccessor.HttpContext;

            if (context != null)
            {
                foreach (var source in _sources.OrderBy(s => s.Order))
                {
                    var timeZone = source.Resolve(context);

                    if (timeZone != null)
                        return timeZone;
                }
            }

            return TimeZoneHelpers.GetTimeZoneById(_options.DefaultZoneId);
        }
    }
}