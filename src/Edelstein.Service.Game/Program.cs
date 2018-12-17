using Edelstein.Core.Services.Startup;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.Game
{
    internal static class Program
    {
        private static void Main(string[] args)
            => new ServiceBootstrap<WvsGame>()
                .WithLogging(() => new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsGame", new WvsGameOptions())
                .WithDistributed()
                .Run()
                .Wait();
    }
}