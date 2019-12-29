using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Caching
{
    public class InMemoryCachingProvider : IProvider
    {
        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<ICacheClient, InMemoryCacheClient>();
            collection.AddSingleton<IMessageBus, InMemoryMessageBus>();
            collection.AddSingleton<IMessageBusFactory, InMemoryMessageBusFactory>();
        }
    }
}