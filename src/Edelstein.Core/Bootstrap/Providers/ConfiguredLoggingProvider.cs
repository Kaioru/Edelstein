using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredLoggingProvider : IProvider
    {
        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(context.Configuration)
                .CreateLogger();
        }
    }
}