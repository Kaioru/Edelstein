using Edelstein.Core.Utils.Messaging;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers.Distribution
{
    public class InMemoryDistributionProvider : AbstractDistributionProvider
    {
        public InMemoryDistributionProvider(string connectionString) : base(connectionString)
        {
        }

        public override void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            collection.AddSingleton<ICacheClient, InMemoryCacheClient>();
            collection.AddSingleton<IMessageBusFactory, InMemoryMessageBusFactory>();
            
            collection .AddSingleton<IMessageBus>(f =>
                f.GetService<IMessageBusFactory>().Build("messages")
            );
            collection.AddSingleton<ILockProvider, CacheLockProvider>();
        }
    }
}