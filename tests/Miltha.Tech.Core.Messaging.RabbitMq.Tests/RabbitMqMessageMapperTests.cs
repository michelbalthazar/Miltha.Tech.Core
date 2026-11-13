using Miltha.Tech.Core.Messaging;
using Miltha.Tech.Core.Messaging.RabbitMq;

namespace Miltha.Tech.Core.Messaging.RabbitMq.Tests
{
    public sealed class RabbitMqMessageMapperTests
    {
        [Fact]
        public void ToProperties_ShouldMapEnvelopeAndHeaders()
        {
            // Arrange
            var message = new IntegrationMessage(
                Guid.NewGuid(),
                "operation.queued",
                "{}",
                "correlation",
                "trace",
                new Dictionary<string, string> { ["tenant"] = "tenant-1" });

            // Act
            var properties = RabbitMqMessageMapper.ToProperties(message);

            // Assert
            Assert.Equal(message.Id.ToString("D"), properties.MessageId);
            Assert.Equal("operation.queued", properties.Type);
            Assert.Equal("correlation", properties.CorrelationId);
            Assert.Equal("application/json", properties.ContentType);
            Assert.True(properties.Persistent);
            Assert.Equal("tenant-1", properties.Headers["tenant"]);
            Assert.Equal("trace", properties.Headers[RabbitMqMessageMapper.TraceIdHeader]);
        }

        [Fact]
        public void FromParts_ShouldRestoreTheEnvelopeAndHeaders()
        {
            // Arrange
            var id = Guid.NewGuid();
            var headers = new Dictionary<string, object>
            {
                [RabbitMqMessageMapper.TraceIdHeader] = "trace"u8.ToArray(),
                ["tenant"] = "tenant-1"
            };

            // Act
            var message = RabbitMqMessageMapper.FromParts(
                id.ToString("D"),
                "operation.queued",
                "correlation",
                "{}",
                headers);

            // Assert
            Assert.Equal(id, message.Id);
            Assert.Equal("operation.queued", message.Type);
            Assert.Equal("correlation", message.CorrelationId);
            Assert.Equal("trace", message.TraceId);
            Assert.Equal("tenant-1", message.Headers["tenant"]);
            Assert.False(message.Headers.ContainsKey(RabbitMqMessageMapper.TraceIdHeader));
        }

        [Fact]
        public void FromParts_ShouldGenerateAnIdWhenMessageIdIsMissing()
        {
            // Act
            var message = RabbitMqMessageMapper.FromParts(null, "type", null, "{}", null);

            // Assert
            Assert.NotEqual(Guid.Empty, message.Id);
            Assert.Null(message.TraceId);
            Assert.Empty(message.Headers);
        }
    }
}
