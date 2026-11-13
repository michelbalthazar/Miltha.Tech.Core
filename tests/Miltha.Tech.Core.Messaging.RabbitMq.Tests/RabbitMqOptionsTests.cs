using Miltha.Tech.Core.Messaging.RabbitMq;

namespace Miltha.Tech.Core.Messaging.RabbitMq.Tests
{
    public sealed class RabbitMqOptionsTests
    {
        [Fact]
        public void Validate_ShouldAcceptDefaults()
        {
            // Arrange
            var options = new RabbitMqOptions();

            // Act
            options.Validate();

            // Assert
            Assert.Equal("amqp://guest:guest@localhost:5672/", options.ConnectionString);
            Assert.Equal(10, options.PrefetchCount);
            Assert.Equal(1, options.ConsumerDispatchConcurrency);
        }

        [Theory]
        [InlineData("")]
        [InlineData("http://localhost")]
        public void Validate_ShouldRejectInvalidConnectionStrings(string connectionString)
        {
            // Arrange
            var options = new RabbitMqOptions { ConnectionString = connectionString };

            // Assert
            var exception = Assert.Throws<ArgumentException>(options.Validate);
            Assert.Equal("ConnectionString", exception.ParamName);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        public void Validate_ShouldRejectEmptyNames(string value)
        {
            // Arrange
            var options = new RabbitMqOptions { ExchangeName = value };

            // Assert
            var exception = Assert.Throws<ArgumentException>(options.Validate);
            Assert.Equal("ExchangeName", exception.ParamName);
        }

        [Fact]
        public void Validate_ShouldRejectZeroPrefetch()
        {
            // Arrange
            var options = new RabbitMqOptions { PrefetchCount = 0 };

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
            Assert.Equal("PrefetchCount", exception.ParamName);
        }

        [Fact]
        public void Validate_ShouldRejectZeroConsumerConcurrency()
        {
            // Arrange
            var options = new RabbitMqOptions { ConsumerDispatchConcurrency = 0 };

            // Assert
            var exception = Assert.Throws<ArgumentOutOfRangeException>(options.Validate);
            Assert.Equal("ConsumerDispatchConcurrency", exception.ParamName);
        }
    }
}
