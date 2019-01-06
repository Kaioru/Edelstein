using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;

namespace Edelstein.Service.Shop
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .WithConfig()
                .WithLogger()
                .WithInferredModel()
                .WithInferredDatabase()
                .WithInferredProvider()
                .WithServiceOption<ShopServiceInfo>()
                .WithService<WvsShop>()
                .Start();
    }
}