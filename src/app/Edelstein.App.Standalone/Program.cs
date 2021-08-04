using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Edelstein.App.Standalone
{
    internal static class Program
    {
        private static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureLogging(logging =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .WriteTo.Console()
                        .CreateLogger();

                    logging.ClearProviders();
                    logging.AddSerilog();
                })
                .ConfigureServices(c =>
                {
                    c.AddHostedService<ProgramHost>();
                })
                .RunConsoleAsync();
    }
}
