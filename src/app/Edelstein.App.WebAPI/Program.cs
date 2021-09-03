using System.Threading.Tasks;
using Edelstein.Common.Hosting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Edelstein.App.WebAPI
{
    internal static class Program
    {
        private static Task Main(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog((ctx, logger) => logger.ReadFrom.Configuration(ctx.Configuration))

                .ConfigureDataStore()
                .ConfigureCaching()
                .ConfigureMessaging()
                .ConfigureParser()

                .ConfigureWebHostDefaults(builder => builder.UseStartup<Startup>())
                .RunConsoleAsync();
    }
}