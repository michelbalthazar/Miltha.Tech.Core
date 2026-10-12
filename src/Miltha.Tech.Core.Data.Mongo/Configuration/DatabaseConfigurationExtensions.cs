using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Miltha.Tech.Data.Mongo.Settings;
using MongoDB.Driver;

namespace Miltha.Tech.Data.Mongo.Configuration
{
    public static class DatabaseConfigurationExtensions
    {
        public static IServiceCollection MongoDbRegister(this IServiceCollection services)
        {
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