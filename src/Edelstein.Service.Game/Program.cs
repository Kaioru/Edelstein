using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Core.Gameplay.Migrations.States;

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
                .WithConfig<GameNodeState>("Service")
                .WithService<GameService>()
                .Build()
                .StartAsync();
    }
}