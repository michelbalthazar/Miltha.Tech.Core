using Microsoft.AspNetCore.Http;

namespace Miltha.Tech.Core.Time
{
    public interface ITimeZoneSource
    {
        int Order { get; }

        TimeZoneInfo? Resolve(HttpContext context);
    }
}