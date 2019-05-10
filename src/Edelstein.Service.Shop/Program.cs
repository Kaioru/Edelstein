using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;
using Edelstein.Service.Shop.Services;

namespace Edelstein.Service.Shop
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .WithTemplates(TemplateCollectionType.Shop)
                .Start<ShopService, ShopServiceInfo>(args);
    }
}