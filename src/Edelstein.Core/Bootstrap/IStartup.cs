using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap
{
    public interface IStartup
    {
        IStartup FromJson(string path, bool optional);
        IStartup FromEnvironment(string prefix);
        IStartup FromCommandLine(string[] args);

        IStartup With<T1, T2>() where T1 : class where T2 : class, T1;
        IStartup WithProvider(IProvider provider);
        IStartup WithService<T>() where T : class, IHostedService;
        IStartup WithConfig<T>(string section) where T : class;

        IHost Build();
    }
}