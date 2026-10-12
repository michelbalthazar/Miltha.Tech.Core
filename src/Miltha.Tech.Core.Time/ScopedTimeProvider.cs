namespace Miltha.Tech.Core.Time
{
    /// <summary>
    /// A scoped <see cref="TimeProvider"/> whose local time zone is resolved for the current request.
    /// The UTC clock always delegates to <see cref="TimeProvider.System"/>.
    /// </summary>
    public sealed class ScopedTimeProvider : TimeProvider
    {
        private readonly ITimeZoneProvider _timeZoneProvider;

        public ScopedTimeProvider(ITimeZoneProvider timeZoneProvider)
        {
            _timeZoneProvider = timeZoneProvider;
        }

        public override TimeZoneInfo LocalTimeZone => _timeZoneProvider.GetTimeZone();

        public override DateTimeOffset GetUtcNow() => TimeProvider.System.GetUtcNow();
    }
}