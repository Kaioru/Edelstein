using Foundatio.Messaging;
using Foundatio.Serializer;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Edelstein.Common.Hosting.Extensions
{
    public static class ServiceCollectionMessagingExtensions
    {
        public static void AddInMemoryMessaging(this IServiceCollection c)
        {
            c.AddSingleton<IMessageBus, InMemoryMessageBus>();
        }

        public static void AddRedisMessaging(this IServiceCollection c, string configuration)
        {
            c.AddSingleton<IConnectionMultiplexer>(ConnectionMultiplexer.Connect(configuration));
            c.AddSingleton<IMessageBus>(p => new RedisMessageBus(new RedisMessageBusOptions
            {
                Serializer = new JsonNetSerializer(
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All
                    }
                ),
                Subscriber = p.GetRequiredService<IConnectionMultiplexer>().GetSubscriber()
            }));
        }
    }
}
