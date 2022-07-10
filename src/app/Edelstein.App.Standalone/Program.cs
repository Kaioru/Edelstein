using System.Threading.Tasks;
using Edelstein.Common.Hosting;
using Microsoft.Extensions.Configuration;
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
                .ConfigureAppConfiguration((context, builder) =>
                {
                    builder.AddJsonFile("appsettings.json", true);
                    builder.AddJsonFile($"appsettings.{context.HostingEnvironment.EnvironmentName}.json", true);
                    builder.AddCommandLine(args);
                })

                .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))

                .ConfigureDataStore()
                .ConfigureCaching()
                .ConfigureMessaging()
                .ConfigureParser()
                .ConfigureScripting()

                .ConfigureServices((context, builder) =>
                {
                    builder.Configure<ProgramConfig>(context.Configuration.GetSection("Host"));
                    builder.AddHostedService<ProgramHost>();
                })
                .RunConsoleAsync();
    }
}
