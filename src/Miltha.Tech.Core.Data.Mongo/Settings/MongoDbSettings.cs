namespace Miltha.Tech.Core.Data.Mongo.Settings
{
    public record MongoDbSettings(string DatabaseName, string Host, string Port)
    {
        public string ConnectionString => $"mongodb://{Host}:{Port}";
    }
}