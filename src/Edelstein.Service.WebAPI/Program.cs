using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;

namespace Edelstein.Service.WebAPI
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .ForService<WebAPIService, WebAPIInfo>()
                .StartAsync();
    }
}