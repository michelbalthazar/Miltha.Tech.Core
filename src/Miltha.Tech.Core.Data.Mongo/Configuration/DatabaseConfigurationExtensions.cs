using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miltha.Tech.Core.Data.Mongo.Serialization;
using Miltha.Tech.Core.Data.Mongo.Settings;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Miltha.Tech.Core.Data.Mongo.Configuration
{
    public static class DatabaseConfigurationExtensions
    {
        public static IServiceCollection MongoDbRegister(this IServiceCollection services)
        {
            BsonSerializer.TryRegisterSerializer(typeof(DateTimeOffset), new UtcDateTimeOffsetSerializer());

            services.AddSingleton<IMongoClient>(x =>
            {
                var settings = x.GetRequiredService<IOptions<MongoDbSettings>>().Value;

                return new MongoClient(settings.ConnectionString);
            });

            services.AddSingleton<IMongoDatabase>(x =>
            {
                var settings = x.GetRequiredService<IOptions<MongoDbSettings>>().Value;

                var mongoClient = x.GetRequiredService<IMongoClient>();

                return mongoClient.GetDatabase(settings.DatabaseName);
            });

            return services;
        }
    }
}