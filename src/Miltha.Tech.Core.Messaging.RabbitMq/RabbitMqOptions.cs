namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Configures the RabbitMQ adapter topology, prefetch and confirmation behavior.</summary>
    public sealed class RabbitMqOptions
    {
        /// <summary>Gets or sets the AMQP connection string.</summary>
        public string ConnectionString { get; set; } = "amqp://guest:guest@localhost:5672/";

        /// <summary>Gets or sets the durable exchange that receives published messages.</summary>
        public string ExchangeName { get; set; } = "miltha.events";

        /// <summary>Gets or sets the dead-letter exchange name.</summary>
        public string DeadLetterExchangeName { get; set; } = "miltha.events.dlx";

        /// <summary>Gets or sets the durable queue consumed by the worker.</summary>
        public string QueueName { get; set; } = "miltha.operations";

        /// <summary>Gets or sets the dead-letter queue name.</summary>
        public string DeadLetterQueueName { get; set; } = "miltha.operations.dlq";

        /// <summary>Gets or sets the binding routing key used by the main queue.</summary>
        public string RoutingKey { get; set; } = "#";

        /// <summary>Gets or sets the maximum number of unacknowledged messages per consumer.</summary>
        public ushort PrefetchCount { get; set; } = 10;

        /// <summary>Gets or sets the consumer dispatch concurrency.</summary>
        public ushort ConsumerDispatchConcurrency { get; set; } = 1;

        /// <summary>Validates the configured topology and connection values.</summary>
        /// <exception cref="ArgumentException">Thrown when a required value is missing or malformed.</exception>
        public void Validate()
        {
            if (!Uri.TryCreate(ConnectionString, UriKind.Absolute, out var uri)
                || (uri.Scheme != "amqp" && uri.Scheme != "amqps"))
                throw new ArgumentException("A valid amqp or amqps connection string is required.", nameof(ConnectionString));

            ValidateName(ExchangeName, nameof(ExchangeName));
            ValidateName(DeadLetterExchangeName, nameof(DeadLetterExchangeName));
            ValidateName(QueueName, nameof(QueueName));
            ValidateName(DeadLetterQueueName, nameof(DeadLetterQueueName));
            ValidateName(RoutingKey, nameof(RoutingKey));

            if (PrefetchCount == 0)
                throw new ArgumentOutOfRangeException(nameof(PrefetchCount), "The prefetch count must be greater than zero.");

            if (ConsumerDispatchConcurrency == 0)
                throw new ArgumentOutOfRangeException(
                    nameof(ConsumerDispatchConcurrency),
                    "The consumer dispatch concurrency must be greater than zero.");
        }

        private static void ValidateName(string value, string parameterName)
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("The value is required.", parameterName);
        }
    }
}
