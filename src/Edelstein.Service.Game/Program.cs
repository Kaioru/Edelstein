using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Services.Distributed.States;
using Edelstein.Service.Game.Service;

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
                .WithConfig<GameServiceState>("Service")
                .WithService<GameService>()
                .Build()
                .StartAsync();
    }
}