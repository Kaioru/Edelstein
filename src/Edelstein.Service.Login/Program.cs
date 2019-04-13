using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;

namespace Edelstein.Service.Login
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .Start<WvsLogin, LoginServiceInfo>(args);
    }
}