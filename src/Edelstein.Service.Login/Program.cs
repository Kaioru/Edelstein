using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Services.Distributed.States;
using Edelstein.Service.Login.Service;

namespace Edelstein.Service.Login
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithConfiguredLogging()
                .WithConfiguredCaching()
                .WithConfiguredDatabase()
                .WithConfig<LoginServiceState>("Service")
                .WithService<LoginService>()
                .Build()
                .StartAsync();
    }
}