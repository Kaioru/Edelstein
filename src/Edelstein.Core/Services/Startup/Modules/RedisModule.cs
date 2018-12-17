using Autofac;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using StackExchange.Redis;

namespace Edelstein.Core.Services.Startup.Modules
{
    public class RedisModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.Register(c =>
                    new RedisHybridCacheClient(o =>
                        o.ConnectionMultiplexer(c.Resolve<ConnectionMultiplexer>())
                    )
                )
                .As<ICacheClient>();
            builder.Register(c =>
                new RedisMessageBus(o =>
                    o.Subscriber(c.Resolve<ConnectionMultiplexer>().GetSubscriber())
                )
            ).As<IMessageBus>();
            builder.RegisterType<CacheLockProvider>().As<ILockProvider>().SingleInstance();
        }
    }
}