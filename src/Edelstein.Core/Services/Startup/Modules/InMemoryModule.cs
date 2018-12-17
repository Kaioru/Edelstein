using Autofac;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Core.Services.Startup.Modules
{
    public class InMemoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<InMemoryCacheClient>().As<ICacheClient>().SingleInstance();
            builder.RegisterType<InMemoryMessageBus>().As<IMessageBus>().SingleInstance();
        }
    }
}