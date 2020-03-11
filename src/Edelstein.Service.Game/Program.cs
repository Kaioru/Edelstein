using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Core.Distributed.States;

namespace Edelstein.Service.Game
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
                .WithProvider(new DataTemplateProvider(DataTemplateType.Game))
                .WithConfig<GameServiceState>("Service")
                .WithService<GameService>()
                .Build()
                .StartAsync();
    }
}