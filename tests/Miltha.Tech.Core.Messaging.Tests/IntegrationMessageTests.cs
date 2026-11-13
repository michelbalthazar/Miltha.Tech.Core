using Miltha.Tech.Core.Messaging;

namespace Miltha.Tech.Core.Messaging.Tests
{
    public sealed class IntegrationMessageTests
    {
        [Fact]
        public void IntegrationMessage_ShouldExposeTheEnvelopeValues()
        {
            // Arrange
            var id = Guid.NewGuid();
            var headers = new Dictionary<string, string> { ["tenant"] = "tenant-1" };

            // Act
            var message = new IntegrationMessage(id, "operation.queued", "{}", "correlation", "trace", headers);

            // Assert
            Assert.Equal(id, message.Id);
            Assert.Equal("operation.queued", message.Type);
            Assert.Equal("{}", message.Payload);
            Assert.Equal("correlation", message.CorrelationId);
            Assert.Equal("trace", message.TraceId);
            Assert.Same(headers, message.Headers);
        }

        [Fact]
        public void IntegrationMessage_ShouldDefaultOptionalValuesToNull()
        {
            // Act
            var message = new IntegrationMessage(Guid.NewGuid(), "type", "{}");

            // Assert
            Assert.Null(message.CorrelationId);
            Assert.Null(message.TraceId);
            Assert.Null(message.Headers);
        }

        [Fact]
        public void IntegrationMessage_ShouldUseValueEquality()
        {
            // Arrange
            var id = Guid.NewGuid();
            var first = new IntegrationMessage(id, "type", "{}");
            var second = new IntegrationMessage(id, "type", "{}");

            // Assert
            Assert.Equal(first, second);
        }

        [Fact]
        public void MessageDisposition_ShouldExposeEveryBrokerOutcome()
        {
            // Assert
            Assert.Equal(
                [MessageDisposition.Acknowledge, MessageDisposition.Requeue, MessageDisposition.Reject],
                Enum.GetValues<MessageDisposition>());
        }
    }
}
