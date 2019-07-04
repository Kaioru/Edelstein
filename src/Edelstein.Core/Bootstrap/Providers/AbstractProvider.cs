using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public abstract class AbstractProvider : IProvider
    {
        public abstract void Provide(HostBuilderContext context, IServiceCollection collection);
    }
}