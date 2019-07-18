using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;
using Edelstein.Service.Login.Services;

namespace Edelstein.Service.Login
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithProvider(new TemplateProvider(TemplateCollectionType.Login))
                .ForService<LoginService, LoginServiceInfo>()
                .StartAsync();
    }
}