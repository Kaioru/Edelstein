using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Commands;
using Edelstein.Core.Services.Info;
using Edelstein.Core.Services.Migrations;
using Edelstein.Data.Context;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Conversations.Scripts;
using Edelstein.Service.Game.Field;
using Edelstein.Service.Game.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Edelstein.Service.Game
{
    public class WvsGame : AbstractMigrateableService<GameServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }
        public IScriptConversationManager ConversationManager { get; }

        public FieldManager FieldManager { get; }
        public CommandRegistry CommandRegistry { get; }

        public WvsGame(
            GameServiceInfo info,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager,
            IScriptConversationManager conversationManager
        ) : base(info, cache, messageBus, dataContextFactory)
        {
            TemplateManager = templateManager;
            ConversationManager = conversationManager;
            FieldManager = new FieldManager(TemplateManager);
            CommandRegistry = new GameCommandRegistry(
                new Parser(settings =>
                {
                    settings.CaseSensitive = false;
                    settings.CaseInsensitiveEnumValues = true;
                })
            );
        }

        public WvsGame(
            WvsGameOptions options,
            ICacheClient cache,
            IMessageBus messageBus,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager,
            IScriptConversationManager conversationManager
        ) : this(options.Service, cache, messageBus, dataContextFactory, templateManager, conversationManager)
        {
        }

        public override async Task Start()
        {
            Server = new Server(new WvsGameSocketFactory(this));

            await base.Start();
            await Server.Start(Info.Host, Info.Port);
        }

        public override async Task Stop()
        {
            await base.Stop();
            await Server.Stop();
        }
    }
}