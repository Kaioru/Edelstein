using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Service.Social.Services;

namespace Edelstein.Service.Social
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .ForService<SocialService, SocialServiceInfo>()
                .StartAsync();
    }
}