using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;
using Edelstein.Service.Shop.Services;

namespace Edelstein.Service.Shop
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithProvider(new TemplateProvider(TemplateCollectionType.Shop))
                .ForService<ShopService, ShopServiceInfo>()
                .StartAsync();
    }
}