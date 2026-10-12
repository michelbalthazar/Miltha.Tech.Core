namespace Miltha.Tech.Core.Time
{
    public sealed class TimeZoneOptions
    {
        public string DefaultZoneId { get; set; } = "Etc/UTC";

        public string HeaderName { get; set; } = "X-Timezone";

        public string ClaimType { get; set; } = "tz";
    }
}