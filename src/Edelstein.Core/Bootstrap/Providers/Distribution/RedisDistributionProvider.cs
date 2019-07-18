using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Foundatio.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Edelstein.Core.Bootstrap.Providers.Distribution
{
    public class RedisDistributionProvider : AbstractDistributionProvider
    {
        public RedisDistributionProvider(string connectionString) : base(connectionString)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            DefaultSerializer.Instance = new JsonNetSerializer(
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                }
            );

            collection.AddSingleton<ConnectionMultiplexer>(f =>
                ConnectionMultiplexer.Connect(ConnectionString)
            );
            collection.AddSingleton<RedisCacheClientOptions>(f => new RedisCacheClientOptions
            {
                ConnectionMultiplexer = f.GetService<ConnectionMultiplexer>()
            });

            collection.AddSingleton<IMessageBusFactory, RedisMessageBusFactory>();
            collection.AddSingleton<ICacheClient, RedisHybridCacheClient>();
            
            collection .AddSingleton<IMessageBus>(f =>
                f.GetService<IMessageBusFactory>().Build("messages")
            );
            collection.AddSingleton<ILockProvider, CacheLockProvider>();
        }
    }
}