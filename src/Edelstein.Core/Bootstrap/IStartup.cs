using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap
{
    public interface IStartup
    {
        IStartup From(StartupOptions options);
        IStartup FromConfiguration(
            string[] args,
            string host = "hostsettings.json",
            string hostEnv = "HOST_",
            string app = "appsettings.json",
            string appEnv = "APP_"
        );
        IStartup WithProvider(IProvider provider);
        IHost ForService<TService, TOption>()
            where TService : class, IHostedService
            where TOption : class;
    }
}