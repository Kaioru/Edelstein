using Edelstein.Core.Services.Startup;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.Login
{
    internal static class Program
    {
        private static void Main(string[] args)
            => ServiceBootstrap<WvsLogin>.Build()
                .WithLogging(new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsLogin", new WvsLoginOptions())
                .WithDistributed()
                .WithMySQLDatabase()
                .WithWZProvider()
                .Run()
                .Wait();
    }
}