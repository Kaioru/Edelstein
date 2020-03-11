using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Service.All.Services;

namespace Edelstein.Service.All
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithConfiguredLogging()
                .WithConfiguredCaching()
                .WithConfiguredDatabase()
                .WithConfiguredParsing()
                .WithConfiguredScripting()
                .WithProvider(new DataTemplateProvider(DataTemplateType.All))
                .WithConfig<ContainerServiceState>("Service")
                .WithService<ContainerService>()
                .Build()
                .StartAsync();
    }
}