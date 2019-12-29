using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.States;
using Edelstein.Service.Login.Services;

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