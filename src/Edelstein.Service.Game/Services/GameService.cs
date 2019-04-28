using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Scripts;
using Edelstein.Core.Scripts.Lua;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Scripted;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Fields;
using Foundatio.Caching;
using Microsoft.Extensions.Options;
using MoonSharp.Interpreter;
using MoreLinq;

namespace Edelstein.Service.Game.Services
{
    public class GameService : AbstractMigrateableService<GameServiceInfo>
    {
        public IDataStore DataStore { get; }
        public ITemplateManager TemplateManager { get; }
        public IScriptManager ScriptManager { get; }

        public IConversationManager ConversationManager { get; }
        public FieldManager FieldManager { get; }

        public GameService(
            IOptions<GameServiceInfo> info,
            ICacheClient cacheClient,
            IMessageBusFactory messageBusFactory,
            IDataStore dataStore,
            ITemplateManager templateManager,
            IScriptManager scriptManager
        ) : base(info.Value, cacheClient, messageBusFactory)
        {
            DataStore = dataStore;
            TemplateManager = templateManager;
            ScriptManager = scriptManager;

            if (scriptManager is LuaScriptManager)
                Speakers.Types.ForEach(t => UserData.RegisterType(t));

            ConversationManager = new ScriptedConversationManager(scriptManager);
            FieldManager = new FieldManager(templateManager);
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new GameSocket(channel, seqSend, seqRecv, this);
    }
}