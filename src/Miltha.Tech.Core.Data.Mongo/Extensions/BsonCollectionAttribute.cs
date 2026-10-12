namespace Miltha.Tech.Data.Mongo.Extensions
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = false)]
    public sealed class BsonCollectionAttribute : Attribute
    {
        public string CollectionName { get; }

        public BsonCollectionAttribute(string collectionName) =>
            CollectionName = collectionName ?? throw new ArgumentNullException(nameof(collectionName));
    }
}