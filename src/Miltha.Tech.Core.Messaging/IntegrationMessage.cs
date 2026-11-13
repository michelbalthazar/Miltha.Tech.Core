namespace Miltha.Tech.Core.Messaging
{
    /// <summary>Represents one integration message exchanged through a message broker.</summary>
    public sealed record IntegrationMessage(
        Guid Id,
        string Type,
        string Payload,
        string CorrelationId = null,
        string TraceId = null,
        IReadOnlyDictionary<string, string> Headers = null);
}
