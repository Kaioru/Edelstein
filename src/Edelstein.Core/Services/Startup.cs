using System.IO;
using System.Threading.Tasks;
using Edelstein.Data.Context;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Parser.NX;
using Edelstein.Provider.Templates;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using StackExchange.Redis;

namespace Edelstein.Core.Services
{
    public class Startup
    {
        public IHostBuilder Builder { get; }

        public Startup()
            => Builder = new HostBuilder();

        public Startup WithConfig(string name = "appsettings")
        {
            Builder.ConfigureAppConfiguration((context, builder) =>
            {
                var env = context.HostingEnvironment;

                builder
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile($"{name}.json", false)
                    .AddJsonFile($"{name}.{env.EnvironmentName}.json", true);
            });
            return this;
        }

        public Startup WithLogger()
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                .CreateLogger();
            return this;
        }

        public Startup WithInferredModel()
        {
            Builder.ConfigureServices((context, services) =>
            {
                switch (context.Configuration["model"].ToLower())
                {
                    case "redis":
                        WithRedisModel(context, services);
                        break;
                    default:
                        WithInMemoryModel(context, services);
                        break;
                }
            });
            return this;
        }

        public Startup WithInMemoryModel(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<ICacheClient, InMemoryCacheClient>();
            services.AddSingleton<IMessageBus, InMemoryMessageBus>();
            services.AddSingleton<ILockProvider, CacheLockProvider>();
            return this;
        }

        public Startup WithRedisModel(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<ConnectionMultiplexer>(f =>
                ConnectionMultiplexer.Connect(context.Configuration.GetSection("connectionString")["Redis"])
            );
            services.AddSingleton<RedisCacheClientOptions>(f => new RedisCacheClientOptions
            {
                ConnectionMultiplexer = f.GetService<ConnectionMultiplexer>()
            });
            services.AddSingleton<RedisMessageBusOptions>(f => new RedisMessageBusOptions
            {
                Subscriber = f.GetService<ConnectionMultiplexer>().GetSubscriber()
            });
            services.AddSingleton<ICacheClient, RedisHybridCacheClient>();
            services.AddSingleton<IMessageBus, RedisMessageBus>();
            services.AddSingleton<ILockProvider, CacheLockProvider>();
            return this;
        }

        public Startup WithInferredDatabase()
        {
            Builder.ConfigureServices((context, services) =>
            {
                switch (context.Configuration["database"].ToLower())
                {
                    case "mysql":
                    default:
                        WithMySQLDatabase(context, services);
                        break;
                }
            });
            return this;
        }

        public Startup WithMySQLDatabase(HostBuilderContext context, IServiceCollection services)
        {
            services.AddSingleton<IDataContextFactory>(f =>
                new MySQLDataContextFactory(context.Configuration.GetSection("connectionString")["MySQL"])
            );
            return this;
        }

        public Startup WithInferredProvider()
        {
            Builder.ConfigureServices((context, services) =>
            {
                switch (context.Configuration["provider"].ToLower())
                {
                    case "nx":
                    default:
                        WithNXProvider(context, services);
                        break;
                }
            });
            return this;
        }

        public Startup WithNXProvider(HostBuilderContext context, IServiceCollection services)
        {
            var collection = new NXDataDirectoryCollection(context.Configuration["dataPath"]);
            var manager = new TemplateManager(collection);

            manager.Load().Wait();
            services.AddSingleton<IDataDirectoryCollection>(collection);
            services.AddSingleton<ITemplateManager>(manager);
            return this;
        }

        public Startup WithServiceOption<TOption>() where TOption : class
        {
            Builder.ConfigureServices((context, services) =>
            {
                services.Configure<TOption>(options =>
                    context.Configuration.GetSection("service").Bind(options)
                );
            });
            return this;
        }

        public Startup WithService<TService>() where TService : class, IHostedService
        {
            Builder.ConfigureServices((context, services) => { services.AddHostedService<TService>(); });
            return this;
        }

        public Task Start()
            => Builder
                .UseConsoleLifetime()
                .RunConsoleAsync();
    }
}