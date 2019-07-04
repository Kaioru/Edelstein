using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers;
using Edelstein.Provider.Templates;

namespace Edelstein.Service.All
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithProvider(new TemplateProvider(TemplateCollectionType.All))
                .ForService<ContainerService, ContainerServiceInfo>()
                .StartAsync();
    }
}