namespace Miltha.Tech.Core.Time
{
    public interface IGeoTimeZoneResolver
    {
        TimeZoneInfo? Resolve(string ip);
    }
}