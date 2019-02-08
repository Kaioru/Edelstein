using System;
using System.Collections.Generic;
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
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Fields.Continent;
using Edelstein.Service.Game.Sockets;
using Foundatio.Caching;
using Foundatio.Messaging;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game
{
    public class WvsGame : AbstractMigrateableService<GameServiceInfo>
    {
        private IServer Server { get; set; }
        public ITemplateManager TemplateManager { get; }
        public IScriptConversationManager ConversationManager { get; }

        public FieldManager FieldManager { get; }
        public ContinentManager ContinentManager { get; }
        public CommandRegistry CommandRegistry { get; }

        public WvsGame(
            IApplicationLifetime appLifetime,
            ICacheClient cache,
            IMessageBus messageBus,
            IOptions<GameServiceInfo> info,
            IDataContextFactory dataContextFactory,
            ITemplateManager templateManager,
            IScriptConversationManager conversationManager
        ) : base(appLifetime, cache, messageBus, info, dataContextFactory)
        {
            TemplateManager = templateManager;
            ConversationManager = conversationManager;
            FieldManager = new FieldManager(TemplateManager);
            ContinentManager = new ContinentManager(TemplateManager, FieldManager);
            CommandRegistry = new GameCommandRegistry(
                new Parser(settings =>
                {
                    settings.CaseSensitive = false;
                    settings.CaseInsensitiveEnumValues = true;
                })
            );
        }

        public override Task OnUpdate(DateTime now)
        {
            return Task.WhenAll(new List<Task>
            {
                ContinentManager.OnUpdate(now)
            });
        }

        protected override async Task OnStarted()
        {
            Server = new Server(new WvsGameSocketFactory(this));

            await base.OnStarted();
            await Server.Start(Info.Host, Info.Port);
        }

        protected override async Task OnStopping()
        {
            await base.OnStopping();
            await Server.Stop();
        }
    }
}