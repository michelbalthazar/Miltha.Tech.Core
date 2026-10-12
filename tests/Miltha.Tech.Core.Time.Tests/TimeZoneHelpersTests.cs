using Miltha.Tech.Core.Time;

namespace Miltha.Tech.Core.Time.Tests
{
    public class TimeZoneHelpersTests
    {
        private static readonly TimeZoneInfo SaoPaulo = TimeZoneInfo.FindSystemTimeZoneById("America/Sao_Paulo");

        private static readonly DateTimeOffset Instant = new(2026, 1, 15, 18, 30, 0, TimeSpan.Zero);

        [Fact]
        public void GetTodayInZone_ShouldReflectZoneOffset()
        {
            // Arrange

            // Act
            var today = TimeZoneHelpers.GetTodayInZone(Instant, SaoPaulo);

            // Assert
            Assert.Equal(new DateOnly(2026, 1, 15), today);
        }

        [Fact]
        public void GetStartOfDayInZone_ShouldConvertToUtc()
        {
            // Arrange

            // Act
            var start = TimeZoneHelpers.GetStartOfDayInZone(Instant, SaoPaulo);

            // Assert
            Assert.Equal(TimeSpan.Zero, start.Offset);
            var local = TimeZoneOffsetOf(start, SaoPaulo);
            Assert.Equal(0, local.Hour);
            Assert.Equal(0, local.Minute);
        }

        private static DateTime TimeZoneOffsetOf(DateTimeOffset value, TimeZoneInfo zone) =>
            TimeZoneInfo.ConvertTime(value, zone).DateTime;

        [Fact]
        public void ConvertZoneLocalToUtc_ShouldAccountForDst()
        {
            // Arrange
            var localSummer = new DateTime(2026, 1, 15, 12, 0, 0, DateTimeKind.Unspecified);

            // Act
            var utc = TimeZoneHelpers.ConvertZoneLocalToUtc(localSummer, SaoPaulo);

            // Assert
            Assert.Equal(TimeSpan.Zero, utc.Offset);
            Assert.Equal(15, utc.UtcDateTime.Hour);
        }

        [Fact]
        public void TryGetTimeZone_ShouldRejectInvalidId()
        {
            // Arrange

            // Act
            var found = TimeZoneHelpers.TryGetTimeZone("Not/AZone", out var zone);

            // Assert
            Assert.False(found);
            Assert.Null(zone);
        }

        [Fact]
        public void ScopedTimeProvider_ShouldUseResolvedLocalTimeZone()
        {
            // Arrange
            var provider = new ScopedTimeProvider(new FixedTimeZoneProvider(SaoPaulo));

            // Act

            // Assert
            Assert.Equal("America/Sao_Paulo", provider.LocalTimeZone.Id);
            Assert.True((DateTimeOffset.UtcNow - provider.GetUtcNow()) < TimeSpan.FromSeconds(5));
        }

        private sealed class FixedTimeZoneProvider : ITimeZoneProvider
        {
            public FixedTimeZoneProvider(TimeZoneInfo timeZone)
            {
                TimeZone = timeZone;
            }

            public TimeZoneInfo TimeZone { get; }

            public TimeZoneInfo GetTimeZone() => TimeZone;
        }
    }
}