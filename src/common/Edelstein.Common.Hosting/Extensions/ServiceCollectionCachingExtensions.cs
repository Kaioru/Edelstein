using Foundatio.Caching;
using Foundatio.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Edelstein.Common.Hosting.Extensions
{
    public static class ServiceCollectionCachingExtensions
    {
        public static void AddInMemoryCaching(this IServiceCollection c)
        {
            c.AddSingleton<ICacheClient, InMemoryCacheClient>();
        }

        public static void AddRedisCaching(this IServiceCollection c, string configuration)
        {
            c.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration));
            c.AddSingleton<ICacheClient>(p => new RedisCacheClient(new RedisCacheClientOptions
            {
                Serializer = new JsonNetSerializer(
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                ),
                ConnectionMultiplexer = p.GetRequiredService<IConnectionMultiplexer>()
            }));
        }
    }
}
