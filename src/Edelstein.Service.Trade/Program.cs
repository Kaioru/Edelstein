using Edelstein.Core.Services.Startup;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.Trade
{
    internal static class Program
    {
        private static void Main(string[] args)
            => ServiceBootstrap<WvsTrade>.Build()
                .WithLogging(new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsTrade", new WvsTradeOptions())
                .WithDistributed()
                .WithMySQLDatabase()
                .WithNXProvider()
                .Run()
                .Wait();
    }
}