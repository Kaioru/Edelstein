using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Edelstein.Core.Services.Startup.Modules;
using Edelstein.Data.Context;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Parser.WZ;
using Microsoft.Extensions.Configuration;
using PKG1;
using Serilog;
using StackExchange.Redis;
using Logger = Serilog.Core.Logger;

namespace Edelstein.Core.Services.Startup
{
    public class ServiceBootstrap<TService>
        where TService : IService
    {
        private ContainerBuilder _builder;

        private ServiceBootstrap()
        {
            _builder = new ContainerBuilder();
            _builder.RegisterType<TService>().As<IService>();
        }

        public static ServiceBootstrap<TService> Build()
            => new ServiceBootstrap<TService>();

        public ServiceBootstrap<TService> WithLogging(Func<Logger> f)
        {
            Log.Logger = f.Invoke();
            return this;
        }

        public ServiceBootstrap<TService> WithConfig(string name, object obj)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{name}.example.json")
                .AddJsonFile($"{name}.json", true);
            var config = configBuilder.Build();

            config.Bind(obj);
            _builder.RegisterInstance(config).As<IConfigurationRoot>();
            _builder.RegisterInstance(obj).AsSelf();
            return this;
        }

        public ServiceBootstrap<TService> WithInMemory()
        {
            _builder.RegisterModule<InMemoryModule>();
            return this;
        }

        public ServiceBootstrap<TService> WithDistributed(string connection = null)
        {
            _builder.Register(c =>
            {
                if (connection == null)
                    connection = c.Resolve<IConfigurationRoot>()["RedisConnectionString"];
                return ConnectionMultiplexer.Connect(connection);
            });
            _builder.RegisterModule<RedisModule>();
            return this;
        }

        public ServiceBootstrap<TService> WithInMemoryDatabase()
        {
            _builder.RegisterInstance(new InMemoryDataContextFactory("memory")).As<IDataContextFactory>();
            return this;
        }

        public ServiceBootstrap<TService> WithMySQLDatabase(string connection = null)
        {
            _builder.Register(c =>
            {
                if (connection == null)
                    connection = c.Resolve<IConfigurationRoot>()["DatabaseConnectionString"];
                return new MySQLDataContextFactory(connection);
            }).As<IDataContextFactory>();
            return this;
        }
        
        public ServiceBootstrap<TService> WithWZProvider(string path = null)
        {
            _builder.Register(c =>
            {
                if (path == null)
                    path = c.Resolve<IConfigurationRoot>()["BaseWZDirectoryPath"];
                return new WZDataPackageCollection(path);
            }).As<IDataDirectoryCollection>();
            return this;
        }

        public Task Run()
        {
            var container = _builder.Build();
            var service = container.Resolve<IService>();
            return service.Start();
        }
    }
}