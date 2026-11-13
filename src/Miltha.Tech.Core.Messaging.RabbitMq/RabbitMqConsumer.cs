using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Consumes integration messages from RabbitMQ with manual acknowledgements and DLQ routing.</summary>
    public sealed class RabbitMqConsumer : IMessageConsumer
    {
        private readonly RabbitMqConnection _connection;
        private readonly ILogger<RabbitMqConsumer> _logger;
        private readonly RabbitMqOptions _options;

        /// <summary>Initializes the consumer with the shared connection, options and logger.</summary>
        public RabbitMqConsumer(
            RabbitMqConnection connection,
            RabbitMqOptions options,
            ILogger<RabbitMqConsumer> logger)
        {
            _connection = connection;
            _options = options;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task ConsumeAsync(
            string queue,
            Func<IntegrationMessage, CancellationToken, Task<MessageDisposition>> handler,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(queue);
            ArgumentNullException.ThrowIfNull(handler);

            var connection = await _connection.GetAsync(cancellationToken);
            var options = new CreateChannelOptions(
                publisherConfirmationsEnabled: false,
                publisherConfirmationTrackingEnabled: false,
                consumerDispatchConcurrency: _options.ConsumerDispatchConcurrency);
            await using var channel = await connection.CreateChannelAsync(options, cancellationToken);

            await DeclareTopologyAsync(channel, queue, cancellationToken);
            await channel.BasicQosAsync(
                prefetchSize: 0,
                prefetchCount: _options.PrefetchCount,
                global: false,
                cancellationToken: cancellationToken);

            var consumer = new AsyncEventingBasicConsumer(channel);
            consumer.ReceivedAsync += (_, args) => HandleAsync(channel, args, handler, cancellationToken);

            await channel.BasicConsumeAsync(
                queue,
                autoAck: false,
                consumerTag: string.Empty,
                noLocal: false,
                exclusive: false,
                arguments: null,
                consumer: consumer,
                cancellationToken: cancellationToken);

            try
            {
                await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
            }
        }

        private async Task DeclareTopologyAsync(
            IChannel channel,
            string queue,
            CancellationToken cancellationToken)
        {
            await channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                passive: false,
                noWait: false,
                cancellationToken: cancellationToken);
            await channel.ExchangeDeclareAsync(
                exchange: _options.DeadLetterExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                passive: false,
                noWait: false,
                cancellationToken: cancellationToken);
            await channel.QueueDeclareAsync(
                queue: _options.DeadLetterQueueName,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null,
                passive: false,
                noWait: false,
                cancellationToken: cancellationToken);
            await channel.QueueBindAsync(
                queue: _options.DeadLetterQueueName,
                exchange: _options.DeadLetterExchangeName,
                routingKey: "#",
                arguments: null,
                noWait: false,
                cancellationToken: cancellationToken);

            var arguments = new Dictionary<string, object>
            {
                ["x-dead-letter-exchange"] = _options.DeadLetterExchangeName
            };
            await channel.QueueDeclareAsync(
                queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: arguments,
                passive: false,
                noWait: false,
                cancellationToken: cancellationToken);
            await channel.QueueBindAsync(
                queue,
                exchange: _options.ExchangeName,
                routingKey: _options.RoutingKey,
                arguments: null,
                noWait: false,
                cancellationToken: cancellationToken);
        }

        private async Task HandleAsync(
            IChannel channel,
            BasicDeliverEventArgs args,
            Func<IntegrationMessage, CancellationToken, Task<MessageDisposition>> handler,
            CancellationToken cancellationToken)
        {
            if (cancellationToken.IsCancellationRequested)
                return;

            var message = RabbitMqMessageMapper.FromDelivery(args);
            MessageDisposition disposition;

            try
            {
                disposition = await handler(message, cancellationToken);
            }
            catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                _logger.LogError(
                    exception,
                    "Message {MessageId} of type {MessageType} failed; requeueing.",
                    message.Id,
                    message.Type);
                disposition = MessageDisposition.Requeue;
            }

            switch (disposition)
            {
                case MessageDisposition.Acknowledge:
                    await channel.BasicAckAsync(args.DeliveryTag, multiple: false, cancellationToken);
                    break;
                case MessageDisposition.Reject:
                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: false,
                        cancellationToken);
                    break;
                default:
                    await channel.BasicNackAsync(
                        args.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken);
                    break;
            }
        }
    }
}
