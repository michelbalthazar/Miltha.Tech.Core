namespace Miltha.Tech.Core.Time
{
    public static class TimeZoneHelpers
    {
        public static bool TryGetTimeZone(string? zoneId, out TimeZoneInfo? timeZone)
        {
            timeZone = null;

            if (string.IsNullOrWhiteSpace(zoneId))
                return false;

            try
            {
                timeZone = TimeZoneInfo.FindSystemTimeZoneById(zoneId);
                return true;
            }
            catch (TimeZoneNotFoundException)
            {
                return false;
            }
            catch (InvalidTimeZoneException)
            {
                return false;
            }
        }

        public static TimeZoneInfo GetTimeZoneById(string zoneId) =>
            TimeZoneInfo.FindSystemTimeZoneById(zoneId);

        public static DateTimeOffset ConvertZoneLocalToUtc(DateTime localDateTime, TimeZoneInfo timeZone)
        {
            var utc = TimeZoneInfo.ConvertTimeToUtc(localDateTime, timeZone);

            return new DateTimeOffset(utc, TimeSpan.Zero);
        }

        public static DateTimeOffset ConvertFromUtc(DateTimeOffset instant, TimeZoneInfo timeZone) =>
            TimeZoneInfo.ConvertTime(instant, timeZone);

        public static DateOnly GetTodayInZone(DateTimeOffset now, TimeZoneInfo timeZone) =>
            DateOnly.FromDateTime(TimeZoneInfo.ConvertTime(now, timeZone).DateTime);

        public static DateTimeOffset GetStartOfDayInZone(DateTimeOffset now, TimeZoneInfo timeZone)
        {
            var localNow = TimeZoneInfo.ConvertTime(now, timeZone);

            var localMidnight = new DateTime(localNow.Year, localNow.Month, localNow.Day, 0, 0, 0, DateTimeKind.Unspecified);

            var utcMidnight = TimeZoneInfo.ConvertTimeToUtc(localMidnight, timeZone);

            return new DateTimeOffset(utcMidnight, TimeSpan.Zero);
        }
    }
}