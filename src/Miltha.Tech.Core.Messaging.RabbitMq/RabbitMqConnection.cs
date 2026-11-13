using RabbitMQ.Client;

namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Owns the shared RabbitMQ connection and recreates it when it is closed.</summary>
    public sealed class RabbitMqConnection : IAsyncDisposable
    {
        private readonly SemaphoreSlim _gate = new(1, 1);
        private readonly RabbitMqOptions _options;
        private IConnection _connection;

        /// <summary>Initializes the connection owner with validated options.</summary>
        public RabbitMqConnection(RabbitMqOptions options)
        {
            _options = options;
        }

        /// <summary>Returns an open connection, creating it when necessary.</summary>
        public async Task<IConnection> GetAsync(CancellationToken cancellationToken = default)
        {
            if (_connection is { IsOpen: true })
                return _connection;

            await _gate.WaitAsync(cancellationToken);

            try
            {
                if (_connection is { IsOpen: true })
                    return _connection;

                var factory = new ConnectionFactory
                {
                    Uri = new Uri(_options.ConnectionString),
                    AutomaticRecoveryEnabled = true,
                    ClientProvidedName = "Miltha.Tech.Core.Messaging.RabbitMq"
                };

                _connection = await factory.CreateConnectionAsync(cancellationToken);
                return _connection;
            }
            finally
            {
                _gate.Release();
            }
        }

        /// <inheritdoc />
        public async ValueTask DisposeAsync()
        {
            if (_connection is not null)
                await _connection.DisposeAsync();

            _gate.Dispose();
        }
    }
}
