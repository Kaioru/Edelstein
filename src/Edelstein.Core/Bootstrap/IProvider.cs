using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap
{
    public interface IProvider
    {
        void Provide(HostBuilderContext context, IServiceCollection collection);
    }
}