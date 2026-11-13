namespace Miltha.Tech.Core.Messaging
{
    /// <summary>Defines how a consumed message should be acknowledged by the broker.</summary>
    public enum MessageDisposition
    {
        /// <summary>The message was processed and must be acknowledged.</summary>
        Acknowledge,

        /// <summary>The message must be redelivered for another attempt.</summary>
        Requeue,

        /// <summary>The message must be rejected and routed to the dead-letter queue.</summary>
        Reject
    }
}
