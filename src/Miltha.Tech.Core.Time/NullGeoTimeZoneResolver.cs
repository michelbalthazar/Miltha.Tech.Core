namespace Miltha.Tech.Core.Time
{
    public sealed class NullGeoTimeZoneResolver : IGeoTimeZoneResolver
    {
        public TimeZoneInfo? Resolve(string ip) => null;
    }
}