using System;
using System.IO;
using System.Threading.Tasks;
using Autofac;
using Edelstein.Core.Services.Startup.Modules;
using Edelstein.Data.Context;
using Edelstein.Provider.Parser;
using Edelstein.Provider.Parser.NX;
using Edelstein.Provider.Templates;
using Microsoft.Extensions.Configuration;
using Serilog;
using StackExchange.Redis;
using Logger = Serilog.Core.Logger;

namespace Edelstein.Core.Services.Startup
{
    public class ServiceBootstrap<TService>
        where TService : IService
    {
        public ContainerBuilder Builder { get; }

        private ServiceBootstrap()
        {
            Builder = new ContainerBuilder();
            Builder.RegisterType<TService>().As<IService>();
        }

        public static ServiceBootstrap<TService> Build()
            => new ServiceBootstrap<TService>();

        public ServiceBootstrap<TService> WithLogging(Logger logger)
        {
            Log.Logger = logger;
            return this;
        }

        public ServiceBootstrap<TService> WithConfig(string name, object obj)
        {
            var configBuilder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile($"{name}.example.json", true)
                .AddJsonFile($"{name}.json", false);
            var config = configBuilder.Build();

            config.Bind(obj);
            Builder.RegisterInstance(config).As<IConfigurationRoot>();
            Builder.RegisterInstance(obj).AsSelf();
            return this;
        }

        public ServiceBootstrap<TService> WithInMemory()
        {
            Builder.RegisterModule<InMemoryModule>();
            return this;
        }

        public ServiceBootstrap<TService> WithDistributed(string connection = null)
        {
            Builder.Register(c =>
            {
                if (connection == null)
                    connection = c.Resolve<IConfigurationRoot>()["RedisConnectionString"];
                return ConnectionMultiplexer.Connect(connection);
            });
            Builder.RegisterModule<RedisModule>();
            return this;
        }

        public ServiceBootstrap<TService> WithInMemoryDatabase()
        {
            Builder.RegisterInstance(new InMemoryDataContextFactory("memory")).As<IDataContextFactory>();
            return this;
        }

        public ServiceBootstrap<TService> WithMySQLDatabase(string connection = null)
        {
            Builder.Register(c =>
            {
                if (connection == null)
                    connection = c.Resolve<IConfigurationRoot>()["DatabaseConnectionString"];
                return new MySQLDataContextFactory(connection);
            }).As<IDataContextFactory>();
            return this;
        }

        public ServiceBootstrap<TService> WithNXProvider(string path = null)
        {
            Builder.Register(c =>
            {
                path = path ?? c.Resolve<IConfigurationRoot>()["DataDirectoryPath"];

                return new NXDataDirectoryCollection(path);
            }).As<IDataDirectoryCollection>();
            return WithTemplates();
        }

        private ServiceBootstrap<TService> WithTemplates()
        {
            Builder.RegisterType<TemplateManager>().As<ITemplateManager>()
                .OnActivated(async args => await args.Instance.Load());
            return this;
        }

        public ServiceBootstrap<TService> WithAdditional(Action<ContainerBuilder> action)
        {
            action.Invoke(Builder);
            return this;
        }

        public Task Run()
        {
            var container = Builder.Build();
            var service = container.Resolve<IService>();
            return service.Start();
        }
    }
}