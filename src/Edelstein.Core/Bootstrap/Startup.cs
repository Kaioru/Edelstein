using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Edelstein.Core.Bootstrap.Providers.Data;
using Edelstein.Core.Bootstrap.Providers.Database;
using Edelstein.Core.Bootstrap.Providers.Distribution;
using Edelstein.Core.Bootstrap.Providers.Script;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoreLinq;

namespace Edelstein.Core.Bootstrap
{
    public class Startup : IStartup
    {
        private readonly IHostBuilder _builder;
        
        public Startup() : this(new HostBuilder())
        {
        }

        public Startup(IHostBuilder builder)
        {
            _builder = builder;
        }

        private ICollection<IProvider> GetProviders(StartupOptions options)
        {
            var distribution = options.Distribution;
            var database = options.Database;
            var parser = options.DataParser;
            var script = options.Script;
            var distributionProvider = distribution.Type switch {
                StartupDistributionType.InMemory => (IProvider) new InMemoryDistributionProvider(distribution.ConnectionString),
                StartupDistributionType.Redis => (IProvider) new RedisDistributionProvider(distribution.ConnectionString)
                };
            var databaseProvider = database.Type switch {
                StartupDatabaseType.InMemory => (IProvider) new InMemoryDatabaseProvider(database.ConnectionString),
                StartupDatabaseType.LiteDB => (IProvider) new LiteDBDatabaseProvider(database.ConnectionString),
                StartupDatabaseType.PostgreSQL => (IProvider) new PostgreSQLDatabaseProvider(database.ConnectionString)
                };
            var parserProvider = parser.Type switch {
                StartupDataParserType.NX => (IProvider) new NXDataParserProvider(parser.Path)
                };
            var scriptProvider = script.Type switch {
                StartupScriptType.Lua => (IProvider) new LuaScriptProvider(script.Path),
                StartupScriptType.Python => (IProvider) new PythonScriptProvider(script.Path)
                };
            
            return new List<IProvider> {distributionProvider, databaseProvider, parserProvider, scriptProvider};
        }
        
        public IStartup From(StartupOptions options)
        {
            GetProviders(options).ForEach(p => WithProvider(p));
            return this;
        }

        public IStartup FromConfiguration(
            string[] args,
            string host = "hostsettings.json",
            string hostEnv = "HOST_",
            string app = "appsettings.json",
            string appEnv = "APP_"
        )
        {
            _builder
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile(host, true);
                    builder.AddEnvironmentVariables(hostEnv);
                    builder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment.EnvironmentName;
                    var split = app.Split(".").ToList();

                    split.Insert(split.Count - 1, env);
                    
                    var envFile = string.Join(".", split);

                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile(app, true);
                    builder.AddJsonFile(envFile , true);
                    builder.AddEnvironmentVariables(appEnv);
                    builder.AddCommandLine(args);
                })
                .ConfigureServices((context, collection) =>
                {
                    var options = new StartupOptions();
                    
                    context.Configuration.Bind(options);
                    GetProviders(options).ForEach(p => p.Provide(context, collection));
                });
            return this;
        }

        public IStartup WithProvider(IProvider provider)
        {
            _builder.ConfigureServices(provider.Provide);
            return this;
        }

        public IHost ForService<TService, TOption>()
            where TService : class, IHostedService
            where TOption : class
        {
            _builder.ConfigureServices((context, collection) =>
            {
                collection.AddHostedService<TService>();
                collection.Configure<TOption>(options =>
                    context.Configuration
                        .GetSection("service")
                        .Bind(options)
                );
            });
            return _builder.Build();
        }
    }
}