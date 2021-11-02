﻿using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Game.Commands;
using Edelstein.Common.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Stages.Login.Templates;
using Edelstein.Common.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Network.DotNetty.Transport;
using Edelstein.Common.Services;
using Edelstein.Common.Services.Social;
using Edelstein.Common.Util.Ticks;
using Edelstein.Protocol.Datastore;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Util.Ticks;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Serilog;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Scripting;
using Edelstein.Common.Gameplay.Users.Skills.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Dialogs.Templates;

namespace Edelstein.App.Standalone
{
    public class ProgramHost : IHostedService
    {
        private readonly ProgramConfig _config;
        private readonly IDataStore _store;
        private readonly ICacheClient _cache;
        private readonly IMessageBus _messenger;
        private readonly IDataDirectoryCollection _data;
        private readonly IScriptEngine _scripting;

        private readonly ICollection<ITransportAcceptor> _acceptors;

        public ProgramHost(
            IOptions<ProgramConfig> options,
            IDataStore store,
            ICacheClient cache,
            IMessageBus messenger,
            IDataDirectoryCollection data,
            IScriptEngine scripting
        )
        {
            _config = options.Value;
            _store = store;
            _cache = cache;
            _messenger = messenger;
            _data = data;
            _scripting = scripting;
            _acceptors = new List<ITransportAcceptor>();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var collection = new ServiceCollection();

            collection.AddLogging(logging => logging.AddSerilog());

            collection.AddSingleton(_store);
            collection.AddSingleton(_cache);
            collection.AddSingleton(_messenger);
            collection.AddSingleton(_data);
            collection.AddSingleton(_scripting);

            collection.AddSingleton<IServerRegistry, ServerRegistry>();
            collection.AddSingleton<ISessionRegistry, SessionRegistry>();
            collection.AddSingleton<IMigrationRegistry, MigrationRegistry>();
            collection.AddSingleton<IDispatchService, DispatchService>();

            collection.AddSingleton<IInviteService, InviteService>();
            collection.AddSingleton<IGuildService, GuildService>();
            collection.AddSingleton<IPartyService, PartyService>();

            collection.AddSingleton<IAccountRepository, AccountRepository>();
            collection.AddSingleton<IAccountWorldRepository, AccountWorldRepository>();
            collection.AddSingleton<ICharacterRepository, CharacterRepository>();

            collection.AddSingleton<ITickerManager, TickerManager>();

            collection.AddSingleton<ITemplateRepository<WorldTemplate>, WorldTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<ItemTemplate>, ItemTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<ItemStringTemplate>, ItemStringTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<ItemOptionTemplate>, ItemOptionTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<ItemSetTemplate>, ItemSetTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<CharacterSkillTemplate>, CharacterSkillTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<FieldTemplate>, FieldTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<FieldStringTemplate>, FieldStringTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<ContiMoveTemplate>, ContiMoveTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<NPCTemplate>, NPCTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<NPCShopTemplate>, NPCShopTemplateRepository>();
            collection.AddSingleton<ITemplateRepository<MobTemplate>, MobTemplateRepository>();

            var provider = collection.BuildServiceProvider();

            await Task.WhenAll(_config.LoginStages.Select(async loginConfig =>
            {
                var loginCollection = new ServiceCollection();

                loginCollection.AddLogging(logging => logging.AddSerilog());

                loginCollection.AddSingleton<
                    IPacketProcessor<LoginStage, LoginStageUser>,
                    PacketProcessor<LoginStage, LoginStageUser>
                >();
                loginCollection.AddSingleton(p => new LoginStage(
                    loginConfig,
                    p.GetService<ILogger<IStage<LoginStage, LoginStageUser>>>(),
                    provider.GetService<IServerRegistry>(),
                    provider.GetService<ISessionRegistry>(),
                    provider.GetService<IMigrationRegistry>(),
                    provider.GetService<IGuildService>(),
                    provider.GetService<IPartyService>(),
                    provider.GetService<IAccountRepository>(),
                    provider.GetService<IAccountWorldRepository>(),
                    provider.GetService<ICharacterRepository>(),
                    provider.GetService<ITickerManager>(),
                    p.GetService<IPacketProcessor<LoginStage, LoginStageUser>>(),
                    provider.GetService<ITemplateRepository<WorldTemplate>>(),
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
                await acceptor.Accept(loginConfig.Host, loginConfig.Port);
            }));
            await Task.WhenAll(_config.GameStages.Select(async gameConfig =>
            {
                var gameCollection = new ServiceCollection();

                gameCollection.AddLogging(logging => logging.AddSerilog());

                gameCollection.AddSingleton<
                    IPacketProcessor<GameStage, GameStageUser>,
                    PacketProcessor<GameStage, GameStageUser>
                >();
                gameCollection.AddSingleton<ICommandProcessor, CommandProcessor>();

                gameCollection.AddSingleton(p => new GameStage(
                    gameConfig,
                    p.GetService<ILogger<IStage<GameStage, GameStageUser>>>(),
                    provider.GetService<IServerRegistry>(),
                    provider.GetService<ISessionRegistry>(),
                    provider.GetService<IMigrationRegistry>(),
                    provider.GetService<IDispatchService>(),
                    provider.GetService<IInviteService>(),
                    provider.GetService<IGuildService>(),
                    provider.GetService<IPartyService>(),
                    provider.GetService<IAccountRepository>(),
                    provider.GetService<IAccountWorldRepository>(),
                    provider.GetService<ICharacterRepository>(),
                    provider.GetService<ITickerManager>(),
                    p.GetService<IPacketProcessor<GameStage, GameStageUser>>(),
                    p.GetService<ICommandProcessor>(),
                    provider.GetService<ITemplateRepository<ItemTemplate>>(),
                    provider.GetService<ITemplateRepository<ItemStringTemplate>>(),
                    provider.GetService<ITemplateRepository<ItemOptionTemplate>>(),
                    provider.GetService<ITemplateRepository<ItemSetTemplate>>(),
                    provider.GetService<ITemplateRepository<CharacterSkillTemplate>>(),
                    provider.GetService<ITemplateRepository<FieldTemplate>>(),
                    provider.GetService<ITemplateRepository<FieldStringTemplate>>(),
                    provider.GetService<ITemplateRepository<ContiMoveTemplate>>(),
                    provider.GetService<ITemplateRepository<NPCTemplate>>(),
                    provider.GetService<ITemplateRepository<NPCShopTemplate>>(),
                    provider.GetService<ITemplateRepository<MobTemplate>>()
                ));
                gameCollection.AddSingleton<ISessionInitializer, GameSessionInitializer>();
                gameCollection.AddSingleton<ITransportAcceptor>(p => new NettyTransportAcceptor(
                    p.GetService<ISessionInitializer>(),
                    _config.Version,
                    _config.Patch,
                    _config.Locale,
                    provider.GetService<ILogger<ITransportAcceptor>>()
                ));

                var gameProvider = gameCollection.BuildServiceProvider();
                var acceptor = gameProvider.GetService<ITransportAcceptor>();

                gameProvider.GetService<GameStage>();

                _acceptors.Add(acceptor);
                await acceptor.Accept(gameConfig.Host, gameConfig.Port);
            }));
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            await Task.WhenAll(_acceptors.Select(a => a.Close()));
        }
    }
}
