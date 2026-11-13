using System.Text;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Publishes integration messages to RabbitMQ with publisher confirmations.</summary>
    public sealed class RabbitMqPublisher : IMessagePublisher, IAsyncDisposable
    {
        private readonly RabbitMqConnection _connection;
        private readonly SemaphoreSlim _gate = new(1, 1);
        private readonly ILogger<RabbitMqPublisher> _logger;
        private readonly RabbitMqOptions _options;
        private IChannel _channel;

        /// <summary>Initializes the publisher with the shared connection, options and logger.</summary>
        public RabbitMqPublisher(
            RabbitMqConnection connection,
            RabbitMqOptions options,
            ILogger<RabbitMqPublisher> logger)
        {
            _connection = connection;
            _options = options;
            _logger = logger;
        }

        /// <inheritdoc />
        public async Task PublishAsync(
            string routingKey,
            IntegrationMessage message,
            CancellationToken cancellationToken = default)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(routingKey);
            ArgumentNullException.ThrowIfNull(message);

            await _gate.WaitAsync(cancellationToken);

            try
            {
                var channel = await GetChannelAsync(cancellationToken);

                try
                {
                    await channel.BasicPublishAsync(
                        exchange: _options.ExchangeName,
                        routingKey: routingKey,
                        mandatory: true,
                        basicProperties: RabbitMqMessageMapper.ToProperties(message),
                        body: Encoding.UTF8.GetBytes(message.Payload),
                        cancellationToken: cancellationToken);
                }
                catch
                {
                    await ResetChannelAsync();
                    throw;
                }

                _logger.LogDebug(
                    "Published message {MessageId} of type {MessageType} to {Exchange}.",
                    message.Id,
                    message.Type,
                    _options.ExchangeName);
            }
            finally
            {
                _gate.Release();
            }
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            await ResetChannelAsync();
            _gate.Dispose();
        }

        private async Task<IChannel> GetChannelAsync(CancellationToken cancellationToken)
        {
            if (_channel is { IsOpen: true })
                return _channel;

            var connection = await _connection.GetAsync(cancellationToken);
            var options = new CreateChannelOptions(
                publisherConfirmationsEnabled: true,
                publisherConfirmationTrackingEnabled: true);

            _channel = await connection.CreateChannelAsync(options, cancellationToken);
            await _channel.ExchangeDeclareAsync(
                exchange: _options.ExchangeName,
                type: ExchangeType.Topic,
                durable: true,
                autoDelete: false,
                arguments: null,
                passive: false,
                noWait: false,
                cancellationToken: cancellationToken);

            return _channel;
        }

        private async Task ResetChannelAsync()
        {
            if (_channel is null)
                return;

            var channel = _channel;
            _channel = null;
            await channel.DisposeAsync();
        }
    }
}
