using Miltha.Tech.Core.Data.Mongo.Serialization;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace Miltha.Tech.Core.Data.Mongo.Tests
{
    public class UtcDateTimeOffsetSerializerTests
    {
        private sealed class Wrapper
        {
            public DateTimeOffset At { get; set; }
        }

        public UtcDateTimeOffsetSerializerTests()
        {
            BsonSerializer.TryRegisterSerializer(typeof(DateTimeOffset), new UtcDateTimeOffsetSerializer());
        }

        [Fact]
        public void ShouldSerialize_AsScalarBsonDateTime()
        {
            // Arrange
            var value = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.FromHours(-3));

            // Act
            var document = new Wrapper { At = value }.ToBsonDocument();

            // Assert
            Assert.True(document["At"].IsBsonDateTime);
        }

        [Fact]
        public void ShouldRoundTrip_PreservingInstantAndUtcOffset()
        {
            // Arrange
            var value = new DateTimeOffset(2026, 1, 15, 12, 0, 0, TimeSpan.FromHours(-3));

            // Act
            var wrapper = BsonSerializer.Deserialize<Wrapper>(new Wrapper { At = value }.ToBsonDocument());

            // Assert
            Assert.Equal(value.UtcDateTime, wrapper.At.UtcDateTime);
            Assert.Equal(TimeSpan.Zero, wrapper.At.Offset);
        }

        [Fact]
        public void ShouldNormalizeLegacyArray_ToUtc()
        {
            // Arrange
            var instant = new DateTimeOffset(2026, 1, 15, 15, 0, 0, TimeSpan.Zero);
            var legacy = new BsonDocument { ["At"] = new BsonArray([instant.Ticks, -180]) }; // [Ticks, OffsetMinutes]

            // Act
            var wrapper = BsonSerializer.Deserialize<Wrapper>(legacy);

            // Assert
            Assert.Equal(instant.UtcDateTime, wrapper.At.UtcDateTime);
            Assert.Equal(TimeSpan.Zero, wrapper.At.Offset);
        }

        [Fact]
        public void ShouldReadStringForm()
        {
            // Arrange
            var document = new BsonDocument { ["At"] = "2026-01-15T15:00:00Z" };

            // Act
            var wrapper = BsonSerializer.Deserialize<Wrapper>(document);

            // Assert
            Assert.Equal(new DateTimeOffset(2026, 1, 15, 15, 0, 0, TimeSpan.Zero), wrapper.At);
        }
    }
}