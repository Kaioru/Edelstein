using Edelstein.Core.Services.Startup;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.Shop
{
    internal static class Program
    {
        private static void Main(string[] args)
            => ServiceBootstrap<WvsShop>.Build()
                .WithLogging(new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsShop", new WvsShopOptions())
                .WithDistributed()
                .WithMySQLDatabase()
                .WithInferredProvider()
                .Run()
                .Wait();
    }
}