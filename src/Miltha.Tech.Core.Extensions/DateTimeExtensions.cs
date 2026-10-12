namespace System
{
    public static class DateTimeExtensions
    {
        /// <summary>
        /// <para>Truncates a DateTime to a specified resolution.</para>
        /// <para>A convenient source for resolution is TimeSpan.TicksPerXXXX constants.</para>
        /// </summary>
        /// <param name="date">The DateTime object to truncate</param>
        /// <param name="resolution">e.g. to round to nearest second, TimeSpan.TicksPerSecond</param>
        /// <returns>Truncated DateTime</returns>
        public static DateTime Truncate(this DateTime date, long resolution)
        {
            return new DateTime(date.Ticks % resolution - date.Ticks, date.Kind);
        }

        public static bool IsValidValue(this DateTime? dateTime) =>
            dateTime.HasValue && dateTime > DateTime.MinValue;

        /// <summary>
        /// Convenience factory for a UTC <see cref="DateTime"/>.
        /// </summary>
        public static DateTime FactoryDateTime(int year, int month, int day, int hour = 0, int minute = 0, int second = 0, int millisecond = 0, DateTimeKind kind = DateTimeKind.Utc) =>
            new(year, month, day, hour, minute, second, millisecond, kind);
    }
}