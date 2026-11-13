namespace Miltha.Tech.Core.Messaging
{
    /// <summary>Consumes integration messages without coupling the application to a broker.</summary>
    public interface IMessageConsumer
    {
        /// <summary>Consumes messages from a queue until cancellation, acknowledging each disposition.</summary>
        /// <param name="queue">Queue name to consume from.</param>
        /// <param name="handler">Handler that processes one message and returns its disposition.</param>
        /// <param name="cancellationToken">Cancellation token that stops consumption.</param>
        /// <returns>A task that completes when consumption stops.</returns>
        Task ConsumeAsync(
            string queue,
            Func<IntegrationMessage, CancellationToken, Task<MessageDisposition>> handler,
            CancellationToken cancellationToken = default);
    }
}
