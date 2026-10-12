using Miltha.Tech.Core.Validation;
using MongoDB.Bson.Serialization.Attributes;

namespace Miltha.Tech.Core.Data.Mongo.Documents
{
    public record DocumentBase
    {
        [BsonId]
        [RequiredGuid]
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Determines wheter the document is active to use
        /// </summary>
        [RequiredBool]
        public bool IsActive { get; set; } = true;

        /// <summary>
        /// Gets or sets a random value that should change whenever a document is persisted to the store.
        /// Support for concurrency
        /// </summary>
        [RequiredString]
        public string ConcurrencyStamp { get; set; } = Guid.NewGuid().ToString();

        [RequiredDate]
        public DateTimeOffset CreatedOn { get; set; }
    }
}