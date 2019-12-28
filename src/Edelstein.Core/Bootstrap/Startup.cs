using System.IO;
using System.Linq;
using Edelstein.Core.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap
{
    public class Startup : IStartup
    {
        private readonly IHostBuilder _builder;

        public Startup()
            => _builder = new HostBuilder();

        public IStartup FromJson(string path, bool optional)
        {
            _builder.ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment.EnvironmentName;
                var split = path.Split(".").ToList();

                split.Insert(split.Count - 1, env);

                var envPath = string.Join(".", split);

                builder.SetBasePath(Directory.GetCurrentDirectory());
                builder.AddJsonFile(path, optional);
                builder.AddJsonFile(envPath, true);
            });
            return this;
        }

        public IStartup FromEnvironment(string prefix)
        {
            _builder.ConfigureAppConfiguration((context, builder) => builder.AddEnvironmentVariables(prefix));
            return this;
        }

        public IStartup FromCommandLine(string[] args)
        {
            _builder.ConfigureAppConfiguration((context, builder) => builder.AddCommandLine(args));
            return this;
        }

        public IStartup With<T1, T2>() where T1 : class where T2 : class, T1
        {
            _builder.ConfigureServices((context, collection) => collection.AddSingleton<T1, T2>());
            return this;
        }

        public IStartup WithProvider(IProvider provider)
        {
            _builder.ConfigureServices(provider.Provide);
            return this;
        }

        public IStartup WithService<T>() where T : class, IHostedService
        {
            _builder.ConfigureServices((context, collection) =>
            {
                collection.AddSingleton<T>();
                collection.AddHostedService<BackgroundService<T>>();
            });
            return this;
        }

        public IStartup WithConfig<T>(string section) where T : class
        {
            _builder.ConfigureServices((context, collection) =>
                collection.Configure<T>(o => context.Configuration
                    .GetSection(section)
                    .Bind(o)));
            return this;
        }

        public IHost Build()
            => _builder.Build();
    }
}