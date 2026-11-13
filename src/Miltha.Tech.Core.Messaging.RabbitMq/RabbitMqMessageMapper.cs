using System.Text;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Miltha.Tech.Core.Messaging.RabbitMq
{
    /// <summary>Maps integration messages to and from RabbitMQ properties and headers.</summary>
    internal static class RabbitMqMessageMapper
    {
        internal const string TraceIdHeader = "trace_id";

        internal static BasicProperties ToProperties(IntegrationMessage message)
        {
            ArgumentNullException.ThrowIfNull(message);

            return new BasicProperties
            {
                MessageId = message.Id.ToString("D"),
                Type = message.Type,
                CorrelationId = message.CorrelationId,
                ContentType = "application/json",
                ContentEncoding = "utf-8",
                Persistent = true,
                Headers = BuildHeaders(message)
            };
        }

        internal static IntegrationMessage FromParts(
            string messageId,
            string type,
            string correlationId,
            string payload,
            IDictionary<string, object> headers) =>
            new(
                Guid.TryParse(messageId, out var id) ? id : Guid.NewGuid(),
                type,
                payload,
                correlationId,
                ReadTraceId(headers),
                ReadHeaders(headers));

        internal static IntegrationMessage FromDelivery(BasicDeliverEventArgs args) =>
            FromParts(
                args.BasicProperties?.MessageId,
                args.BasicProperties?.Type ?? args.RoutingKey,
                args.BasicProperties?.CorrelationId,
                Encoding.UTF8.GetString(args.Body.Span),
                args.BasicProperties?.Headers);

        private static Dictionary<string, object> BuildHeaders(IntegrationMessage message)
        {
            var headers = new Dictionary<string, object>(StringComparer.Ordinal);

            if (message.Headers is not null)
            {
                foreach (var header in message.Headers)
                    headers[header.Key] = header.Value;
            }

            if (!string.IsNullOrWhiteSpace(message.TraceId))
                headers[TraceIdHeader] = message.TraceId;

            return headers;
        }

        private static string ReadTraceId(IDictionary<string, object> headers) =>
            headers is not null && headers.TryGetValue(TraceIdHeader, out var value)
                ? ReadValue(value)
                : null;

        private static IReadOnlyDictionary<string, string> ReadHeaders(IDictionary<string, object> headers)
        {
            if (headers is null || headers.Count == 0)
                return new Dictionary<string, string>(StringComparer.Ordinal);

            var values = new Dictionary<string, string>(StringComparer.Ordinal);

            foreach (var header in headers)
            {
                if (string.Equals(header.Key, TraceIdHeader, StringComparison.Ordinal))
                    continue;

                values[header.Key] = ReadValue(header.Value);
            }

            return values;
        }

        private static string ReadValue(object value) =>
            value switch
            {
                null => null,
                string text => text,
                byte[] bytes => Encoding.UTF8.GetString(bytes),
                _ => Convert.ToString(value, System.Globalization.CultureInfo.InvariantCulture)
            };
    }
}
