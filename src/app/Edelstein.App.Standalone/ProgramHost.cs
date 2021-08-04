using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Datastore.LiteDB;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Interop;
using Edelstein.Common.Network.DotNetty.Transport;
using Edelstein.Common.Parser.Duey;
using Edelstein.Common.Util.Caching;
using Edelstein.Common.Util.Ticks;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Caching;
using Edelstein.Protocol.Util.Ticks;
using LiteDB;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;

namespace Edelstein.App.Standalone
{
    public class ProgramHost : IHostedService
    {
        private readonly ProgramConfig _config;
        private readonly ICollection<ITransportAcceptor> _acceptors;

        public ProgramHost(IOptions<ProgramConfig> options)
        {
            _config = options.Value;
            _acceptors = new List<ITransportAcceptor>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var collection = new ServiceCollection();

            collection.AddLogging(logging => logging.AddSerilog());

            collection.AddSingleton<ICache>(p => new LocalCache());
            collection.AddSingleton<IDataStore>(p => new LiteDataStore(new LiteRepository(_config.Database)));
            collection.AddSingleton<IDataDirectoryCollection>(p => new NXDataDirectoryCollection(_config.DataPath));

            collection.AddSingleton<IServerRegistryService, ServerRegistryService>();
            collection.AddSingleton<ISessionRegistryService, SessionRegistryService>();
            collection.AddSingleton<IMigrationRegistryService, MigrationRegistryService>();

            collection.AddSingleton<IAccountRepository, AccountRepository>();
            collection.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
            collection.AddSingleton<ICharacterRepository, CharacterRepository>();

            collection.AddSingleton<ITickerManager, TickerManager>();

            collection.AddSingleton<ITemplateRepository<ItemTemplate>, ItemTemplateRepository>();

            var provider = collection.BuildServiceProvider();

            await Task.WhenAll(_config.LoginStages.Select(async c =>
            {
                var loginCollection = new ServiceCollection();

                loginCollection.AddLogging(logging => logging.AddSerilog());

                // TODO: packet processors

                loginCollection.AddSingleton<
                    IPacketProcessor<LoginStage, LoginStageUser>,
                    PacketProcessor<LoginStage, LoginStageUser>
                >();
                loginCollection.AddSingleton(p => new LoginStage(
                    p.GetService<LoginStageConfig>(),
                    provider.GetService<IServerRegistryService>(),
                    provider.GetService<ISessionRegistryService>(),
                    provider.GetService<IMigrationRegistryService>(),
                    provider.GetService<IAccountRepository>(),
                    provider.GetService<IAccountWorldRepository>(),
                    provider.GetService<ICharacterRepository>(),
                    provider.GetService<ITickerManager>(),
                    p.GetService<IPacketProcessor<LoginStage, LoginStageUser>>(),
                    provider.GetService<ITemplateRepository<ItemTemplate>>()
                ));
                loginCollection.AddSingleton<ISessionInitializer, LoginSessionInitializer>();
                loginCollection.AddSingleton<ITransportAcceptor>(p => new NettyTransportAcceptor(
                    p.GetService<ISessionInitializer>(),
                    _config.Version,
                    _config.Patch,
                    _config.Locale,
                    provider.GetService<ILogger<ITransportAcceptor>>()
                ));

                var loginProvider = loginCollection.BuildServiceProvider();
                var acceptor = loginProvider.GetService<ITransportAcceptor>();

                loginProvider.GetService<LoginStage>();

                _acceptors.Add(acceptor);
                await acceptor.Accept(c.Host, c.Port);
            }));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_acceptors.Select(a => a.Close()));
        }
    }
}
