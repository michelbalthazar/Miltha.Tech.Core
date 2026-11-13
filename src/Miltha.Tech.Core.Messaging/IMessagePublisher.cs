namespace Miltha.Tech.Core.Messaging
{
    /// <summary>Publishes integration messages without coupling the application to a broker.</summary>
    public interface IMessagePublisher
    {
        /// <summary>Publishes one message under the supplied routing key.</summary>
        /// <param name="routingKey">Broker routing key used to select bound queues.</param>
        /// <param name="message">Message envelope to publish.</param>
        /// <param name="cancellationToken">Cancellation token for the publish operation.</param>
        /// <returns>A task that completes when the broker confirms the message.</returns>
        Task PublishAsync(
            string routingKey,
            IntegrationMessage message,
            CancellationToken cancellationToken = default);
    }
}
