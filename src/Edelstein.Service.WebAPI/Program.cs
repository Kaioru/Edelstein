using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Service.WebAPI
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
                .WithConfig<DefaultNodeState>("Service")
                .WithConfig<WebAPIConfig>("Config")
                .WithService<WebAPIService>()
                .Build()
                .StartAsync();
    }
}