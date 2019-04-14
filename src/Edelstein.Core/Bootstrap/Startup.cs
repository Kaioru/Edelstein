using System.IO;
using System.Threading.Tasks;
using Edelstein.Core.Bootstrap.Types;
using Edelstein.Provider;
using Edelstein.Provider.NX;
using Foundatio.Caching;
using Foundatio.Lock;
using Foundatio.Messaging;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using StackExchange.Redis;

namespace Edelstein.Core.Bootstrap
{
    public class Startup
    {
        private readonly StartupOption _option;

        public Startup()
        {
            _option = new StartupOption();
        }

        public Startup WithDistributed(DistributedType type, string connection = null)
        {
            _option.DistributedType = type;
            _option.DistributedConnectionString = connection;
            return this;
        }

        public Startup WithDatabase(DatabaseType type, string connection = null)
        {
            _option.DatabaseType = type;
            _option.DatabaseConnectionString = connection;
            return this;
        }

        public Startup WithProvider(ProviderType type, string path = null)
        {
            _option.ProviderType = type;
            _option.ProviderFolderPath = path;
            return this;
        }

        public Startup WithScript(ScriptType type, string path = null)
        {
            _option.ScriptType = type;
            _option.ScriptFolderPath = path;
            return this;
        }

        public IHost Build<TService, TOption>(string[] args)
            where TService : class, IHostedService
            where TOption : class
        {
            return new HostBuilder()
                .ConfigureHostConfiguration(builder =>
                {
                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("hostsettings.json", true);
                    builder.AddEnvironmentVariables("HOST_");
                    builder.AddCommandLine(args);
                })
                .ConfigureAppConfiguration((context, builder) =>
                {
                    var env = context.HostingEnvironment.EnvironmentName;

                    builder.SetBasePath(Directory.GetCurrentDirectory());
                    builder.AddJsonFile("appsettings.json", true);
                    builder.AddJsonFile($"appsettings.{env}.json", true);
                    builder.AddEnvironmentVariables("APP_");
                    builder.AddCommandLine(args);
                })
                .ConfigureLogging((context, builder) =>
                {
                    Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Verbose()
                        .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                        .CreateLogger();
                })
                .ConfigureServices((context, builder) =>
                {
                    builder.AddHostedService<TService>();

                    var config = context.Configuration;

                    builder.Configure<TOption>(options =>
                        config.GetSection("service").Bind(options)
                    );

                    config.Bind(_option);

                    switch (_option.DistributedType)
                    {
                        default:
                        case null:
                        case DistributedType.InMemory:
                            builder.AddSingleton<ICacheClient, InMemoryCacheClient>();
                            builder.AddSingleton<IMessageBus, InMemoryMessageBus>();
                            builder.AddSingleton<ILockProvider, CacheLockProvider>();
                            break;
                        case DistributedType.Redis:
                            builder.AddSingleton<ConnectionMultiplexer>(f =>
                                ConnectionMultiplexer.Connect(_option.DistributedConnectionString)
                            );
                            builder.AddSingleton<RedisCacheClientOptions>(f => new RedisCacheClientOptions
                            {
                                ConnectionMultiplexer = f.GetService<ConnectionMultiplexer>()
                            });
                            builder.AddSingleton<RedisMessageBusOptions>(f => new RedisMessageBusOptions
                            {
                                Subscriber = f.GetService<ConnectionMultiplexer>().GetSubscriber()
                            });
                            builder.AddSingleton<ICacheClient, RedisHybridCacheClient>();
                            builder.AddSingleton<IMessageBus, RedisMessageBus>();
                            builder.AddSingleton<ILockProvider, CacheLockProvider>();
                            break;
                    }

                    switch (_option.DatabaseType)
                    {
                        default:
                        case null:
                        case DatabaseType.PostgreSQL:
                            builder.AddSingleton<IDocumentStore>(f =>
                                DocumentStore.For(_option.DatabaseConnectionString)
                            );
                            break;
                    }

                    switch (_option.ProviderType)
                    {
                        default:
                        case null:
                        case ProviderType.NX:
                            builder.AddSingleton<IDataDirectoryCollection>(
                                new NXDataDirectoryCollection(_option.ProviderFolderPath)
                            );
                            break;
                    }
                })
                .Build();
        }

        public Task Start<TService, TOption>(string[] args)
            where TService : class, IHostedService
            where TOption : class
        {
            return Build<TService, TOption>(args).StartAsync();
        }
    }
}