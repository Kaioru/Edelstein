using Foundatio.Caching;
using Foundatio.Messaging;
using Foundatio.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Edelstein.Core.Bootstrap.Providers.Caching
{
    public class RedisCachingProvider : IProvider
    {
        private readonly string _connectionString;

        public RedisCachingProvider(string connectionString)
            => _connectionString = connectionString;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            DefaultSerializer.Instance = new JsonNetSerializer(
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            );

            collection.AddSingleton<ConnectionMultiplexer>(f =>
                ConnectionMultiplexer.Connect(_connectionString)
            );
            collection.AddSingleton<RedisCacheClientOptions>(f => new RedisCacheClientOptions
            {
                ConnectionMultiplexer = f.GetService<ConnectionMultiplexer>()
            });
            collection.AddSingleton<RedisMessageBusOptions>(f => new RedisMessageBusOptions()
            {
                Subscriber = f.GetService<ConnectionMultiplexer>().GetSubscriber()
            });

            collection.AddSingleton<ICacheClient, RedisHybridCacheClient>();
            collection.AddSingleton<IMessageBus, RedisMessageBus>();
        }
    }
}