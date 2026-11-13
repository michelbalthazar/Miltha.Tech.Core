using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Registers the RabbitMQ messaging adapter and its options.</summary>
    public static class RabbitMqServiceCollectionExtensions
    {
        /// <summary>Registers the RabbitMQ connection, publisher and consumer singletons.</summary>
        /// <param name="services">Service collection that receives the messaging services.</param>
        /// <param name="configure">Callback that configures and validates the RabbitMQ options.</param>
        /// <returns>The same service collection for chaining.</returns>
        public static IServiceCollection AddRabbitMqMessaging(
            this IServiceCollection services,
            Action<RabbitMqOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(configure);

            var options = new RabbitMqOptions();
            configure(options);
            options.Validate();

            services.TryAddSingleton(options);
            services.TryAddSingleton<RabbitMqConnection>();
            services.TryAddSingleton<IMessagePublisher, RabbitMqPublisher>();
            services.TryAddSingleton<IMessageConsumer, RabbitMqConsumer>();

            return services;
        }
    }
}
