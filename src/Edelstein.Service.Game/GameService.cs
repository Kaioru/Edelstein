using System;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Database;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Network;
using Edelstein.Core.Provider;
using Edelstein.Core.Scripting;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Packets;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Scripted;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Fields.Continent;
using Edelstein.Service.Game.Handlers;
using Edelstein.Service.Game.Handlers.Mob;
using Edelstein.Service.Game.Handlers.NPC;
using Edelstein.Service.Game.Handlers.Users;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game
{
    public class GameService : AbstractMigrationService<GameNodeState>, ITickBehavior
    {
        private readonly ITicker _ticker;

        public IDataTemplateManager TemplateManager { get; }
        public IConversationManager ConversationManager { get; }
        public ICommand CommandManager { get; }
        public FieldManager FieldManager { get; }
        public ContinentManager ContinentManager { get; }

        public ISocialMemoManager MemoManager { get; }
        public ISocialPartyManager PartyManager { get; }
        public ISocialGuildManager GuildManager { get; }

        public GameService(
            IOptions<GameNodeState> state,
            IDataStore dataStore,
            ICacheClient cache,
            IMessageBusFactory busFactory,
            ILockProvider lockProvider,
            IDataTemplateManager templateManager,
            IScriptManager scriptManager
        ) : base(state.Value, dataStore, cache, busFactory)
        {
            _ticker = new TimerTicker(TimeSpan.FromSeconds(1), this);

            TemplateManager = templateManager;
            ConversationManager = new ScriptedConversationManager(scriptManager);
            CommandManager = new CommandManager(new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.CaseInsensitiveEnumValues = true;
            }));
            FieldManager = new FieldManager(templateManager);
            ContinentManager = new ContinentManager(templateManager, FieldManager);

            MemoManager = new SocialMemoManager(
                this,
                dataStore,
                lockProvider,
                cache
            );
            PartyManager = new SocialPartyManager(
                state.Value.ChannelID,
                this,
                dataStore,
                lockProvider,
                cache
            );
            GuildManager = new SocialGuildManager(
                this,
                dataStore,
                lockProvider,
                cache
            );

            Handlers[RecvPacketOperations.MigrateIn] = new MigrateInHandler();

            Handlers[RecvPacketOperations.UserTransferChannelRequest] = new UserTransferChannelRequestHandler();
            Handlers[RecvPacketOperations.UserMigrateToCashShopRequest] = new UserMigrateToCashShopRequestHandler();
            Handlers[RecvPacketOperations.UserMove] = new UserMoveHandler();
            Handlers[RecvPacketOperations.UserChat] = new UserChatHandler();
            Handlers[RecvPacketOperations.UserEmotion] = new UserEmotionHandler();
            Handlers[RecvPacketOperations.UserSelectNpc] = new UserSelectNPCHandler();
            Handlers[RecvPacketOperations.UserScriptMessageAnswer] = new UserScriptMessageAnswerHandler();
            Handlers[RecvPacketOperations.UserGatherItemRequest] = new UserGatherItemRequestHandler();
            Handlers[RecvPacketOperations.UserSortItemRequest] = new UserSortItemRequestHandler();
            Handlers[RecvPacketOperations.UserChangeSlotPositionRequest] = new UserChangeSlotPositionRequestHandler();
            Handlers[RecvPacketOperations.UserConsumeCashItemUseRequest] = new UserConsumeCashItemUseRequestHandler();
            Handlers[RecvPacketOperations.UserItemReleaseRequest] = new UserItemReleaseRequestHandler();
            Handlers[RecvPacketOperations.UserCharacterInfoRequest] = new UserCharacterInfoRequestHandler();
            Handlers[RecvPacketOperations.UserMigrateToITCRequest] = new UserMigrateToITCRequestHandler();

            Handlers[RecvPacketOperations.GroupMessage] = new GroupMessageHandler();
            Handlers[RecvPacketOperations.PartyRequest] = new PartyRequestHandler();
            Handlers[RecvPacketOperations.GuildRequest] = new GuildRequestHandler();
            Handlers[RecvPacketOperations.MemoRequest] = new MemoRequestHandler();

            Handlers[RecvPacketOperations.MobMove] = new MobMoveHandler();

            Handlers[RecvPacketOperations.NpcMove] = new NPCMoveHandler();

            Handlers[RecvPacketOperations.CONTISTATE] = new ContiStateHandler();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            await this.SubscribeSocialEvents(cancellationToken);
            await this.SubscribePartyEvents(cancellationToken);
            await this.SubscribeGuildEvents(cancellationToken);
            _ticker.Start();
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);

        public async Task TryTick(DateTime now)
        {
            await FieldManager.TryTick(now);
            await ContinentManager.TryTick(now);
        }
    }
}