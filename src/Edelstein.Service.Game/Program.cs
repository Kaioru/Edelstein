using System;
using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Service.Game.Services;

namespace Edelstein.Service.Game
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .Start<GameService, GameServiceInfo>(args);
    }
}