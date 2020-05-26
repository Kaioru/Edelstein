using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Core.Gameplay.Migrations.States;

namespace Edelstein.Service.Shop
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
                .WithProvider(new DataTemplateProvider(DataTemplateType.Shop))
                .WithConfig<ShopNodeState>("Service")
                .WithService<ShopService>()
                .Build()
                .StartAsync();
    }
}