using System;
using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Service.All.Services;

namespace Edelstein.Service.All
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .WithConfiguredLogging()
                .WithConfiguredCaching()
                .WithConfiguredDatabase()
                .WithConfig<ContainerServiceState>("Service")
                .WithService<ContainerService>()
                .Build()
                .StartAsync();
    }
}