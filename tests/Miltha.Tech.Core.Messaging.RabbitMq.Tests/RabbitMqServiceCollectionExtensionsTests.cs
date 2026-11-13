using Microsoft.Extensions.DependencyInjection;
using Miltha.Tech.Core.Messaging;
using Miltha.Tech.Core.Messaging.RabbitMq;

namespace Miltha.Tech.Core.Messaging.RabbitMq.Tests
{
    public sealed class RabbitMqServiceCollectionExtensionsTests
    {
        [Fact]
        public async Task AddRabbitMqMessaging_ShouldRegisterTheAdapter()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddLogging();
            services.AddRabbitMqMessaging(_ => { });

            // Act
            await using var provider = services.BuildServiceProvider();

            // Assert
            Assert.NotNull(provider.GetRequiredService<RabbitMqOptions>());
            Assert.IsType<RabbitMqConnection>(provider.GetRequiredService<RabbitMqConnection>());
            Assert.IsType<RabbitMqPublisher>(provider.GetRequiredService<IMessagePublisher>());
            Assert.IsType<RabbitMqConsumer>(provider.GetRequiredService<IMessageConsumer>());
        }

        [Fact]
        public void AddRabbitMqMessaging_ShouldRejectInvalidOptions()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            var act = () => services.AddRabbitMqMessaging(options => options.ConnectionString = "http://localhost");

            // Assert
            var exception = Assert.Throws<ArgumentException>(act);
            Assert.Equal("ConnectionString", exception.ParamName);
        }
    }
}
