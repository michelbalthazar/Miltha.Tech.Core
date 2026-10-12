using System.Net;

namespace Miltha.Tech.Core.Data.Mongo.Extensions
{
    public sealed class ConcurrencyConflictException : Exception
    {
        public Guid Id { get; }
        public HttpStatusCode StatusCode { get; set; }
        public string Expected { get; }

        public ConcurrencyConflictException(Guid id, string expected)
            : base($"Concurrency conflict for document {id} (expected stamp {expected}).")
        {
            Id = id;
            Expected = expected;
            StatusCode = HttpStatusCode.Conflict;
        }
    }
}