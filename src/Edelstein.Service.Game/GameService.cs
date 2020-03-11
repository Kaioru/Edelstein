using Edelstein.Core.Distributed.States;
using Edelstein.Core.Scripting;
using Edelstein.Core.Services.Migrations;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Provider;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Scripted;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Handlers;
using Edelstein.Service.Game.Handlers.NPC;
using Edelstein.Service.Game.Handlers.Users;
using Foundatio.Caching;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game
{
    public class GameService : AbstractMigrationService<GameServiceState>
    {
        public IDataTemplateManager TemplateManager { get; }
        public IConversationManager ConversationManager { get; }
        public FieldManager FieldManager { get; }

        public GameService(
            IOptions<GameServiceState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory,
            IDataTemplateManager templateManager,
            IScriptManager scriptManager
        ) : base(state.Value, dataStore, cache, busFactory)
        {
            TemplateManager = templateManager;
            ConversationManager = new ScriptedConversationManager(scriptManager);
            FieldManager = new FieldManager(templateManager);

            Handlers[RecvPacketOperations.MigrateIn] = new MigrateInHandler();

            Handlers[RecvPacketOperations.UserTransferChannelRequest] = new UserTransferChannelRequestHandler();
            Handlers[RecvPacketOperations.UserMove] = new UserMoveHandler();
            Handlers[RecvPacketOperations.UserChat] = new UserChatHandler();
            Handlers[RecvPacketOperations.UserGatherItemRequest] = new UserGatherItemRequestHandler();
            Handlers[RecvPacketOperations.UserSortItemRequest] = new UserSortItemRequestHandler();
            Handlers[RecvPacketOperations.UserChangeSlotPositionRequest] = new UserChangeSlotPositionRequestHandler();

            Handlers[RecvPacketOperations.NpcMove] = new NPCMoveHandler();
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);
    }
}