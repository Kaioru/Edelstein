using Edelstein.Core.Services.Startup;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.All
{
    internal static class Program
    {
        private static void Main(string[] args)
            => new ServiceBootstrap<WvsContainer>()
                .WithLogging(() => new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsContainer", new WvsContainerOptions())
                .WithInMemory()
                .Run()
                .Wait();
    }
}