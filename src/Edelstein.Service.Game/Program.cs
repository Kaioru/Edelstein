using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Services;

namespace Edelstein.Service.Game
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithProvider(new TemplateProvider(TemplateCollectionType.Game))
                .ForService<GameService, GameServiceInfo>()
                .StartAsync();
    }
}