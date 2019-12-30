using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Bootstrap.Providers.Templates;
using Edelstein.Core.Distributed.States;

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
                .WithConfiguredParsing()
                .WithProvider(new DataTemplateProvider(DataTemplateType.Login))
                .WithConfig<LoginServiceState>("Service")
                .WithService<LoginService>()
                .Build()
                .StartAsync();
    }
}