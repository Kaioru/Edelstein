using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Gameplay.Social.Party.Events;
using Edelstein.Core.Scripting;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Core.Utils.Packets;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Database;
using Edelstein.Entities.Social;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Scripted;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Fields.Continent;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Handlers;
using Edelstein.Service.Game.Handlers.NPC;
using Edelstein.Service.Game.Handlers.Users;
using Foundatio.Caching;
using Foundatio.Lock;
using Microsoft.Extensions.Options;

namespace Edelstein.Service.Game
{
    public class GameService : AbstractMigrationService<GameServiceState>, ITickBehavior
    {
        private readonly ITicker _ticker;

        public IDataTemplateManager TemplateManager { get; }
        public IConversationManager ConversationManager { get; }
        public ICommand CommandManager { get; }
        public FieldManager FieldManager { get; }
        public ContinentManager ContinentManager { get; }

        public ISocialPartyManager PartyManager { get; }
        public ISocialGuildManager GuildManager { get; }

        public GameService(
            IOptions<GameServiceState> state,
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

            PartyManager = new SocialPartyManager(
                state.Value.ChannelID,
                this,
                dataStore,
                lockProvider,
                cache
            );
            GuildManager = new SocialGuildManager();

            Handlers[RecvPacketOperations.MigrateIn] = new MigrateInHandler();

            Handlers[RecvPacketOperations.UserTransferChannelRequest] = new UserTransferChannelRequestHandler();
            Handlers[RecvPacketOperations.UserMove] = new UserMoveHandler();
            Handlers[RecvPacketOperations.UserChat] = new UserChatHandler();
            Handlers[RecvPacketOperations.UserEmotion] = new UserEmotionHandler();
            Handlers[RecvPacketOperations.UserSelectNpc] = new UserSelectNPCHandler();
            Handlers[RecvPacketOperations.UserScriptMessageAnswer] = new UserScriptMessageAnswerHandler();
            Handlers[RecvPacketOperations.UserGatherItemRequest] = new UserGatherItemRequestHandler();
            Handlers[RecvPacketOperations.UserSortItemRequest] = new UserSortItemRequestHandler();
            Handlers[RecvPacketOperations.UserChangeSlotPositionRequest] = new UserChangeSlotPositionRequestHandler();

            Handlers[RecvPacketOperations.NpcMove] = new NPCMoveHandler();

            Handlers[RecvPacketOperations.CONTISTATE] = new ContiStateHandler();
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            await base.StartAsync(cancellationToken);
            _ticker.Start();

            await Bus.SubscribeAsync<PartyUserMigrationEvent>(
                async (msg, token) =>
                {
                    var users = FieldManager
                        .GetAll()
                        .SelectMany(f => f.GetObjects<FieldUser>())
                        .Where(u => u.Party != null)
                        .Where(u => u.Party.ID == msg.PartyID)
                        .ToImmutableList();

                    await Task.WhenAll(users.Select(u => u.Party.OnUpdateUserMigration(
                        msg.PartyMemberID,
                        msg.ChannelID,
                        msg.FieldID
                    )));
                    await Task.WhenAll(users.Select(async u =>
                    {
                        using var p = new Packet(SendPacketOperations.PartyResult);

                        p.Encode<byte>((byte) PartyResultType.UserMigration);
                        p.Encode<int>(u.Party.ID);
                        u.Party.EncodeData(State.ChannelID, p);

                        await u.SendPacket(p);
                    }));
                }, cancellationToken);
            await Bus.SubscribeAsync<PartyChangeLevelOrJobEvent>(async (msg, token) =>
            {
                var users = FieldManager
                    .GetAll()
                    .SelectMany(f => f.GetObjects<FieldUser>())
                    .Where(u => u.Party != null)
                    .Where(u => u.Party.ID == msg.PartyID)
                    .ToImmutableList();

                await Task.WhenAll(users.Select(u => u.Party.OnUpdateChangeLevelOrJob(
                    msg.PartyMemberID,
                    msg.Level,
                    msg.Job
                )));
                await Task.WhenAll(users.Select(async u =>
                {
                    using var p = new Packet(SendPacketOperations.PartyResult);

                    p.Encode<byte>((byte) PartyResultType.ChangeLevelOrJob);
                    p.Encode<int>(msg.PartyMemberID);
                    p.Encode<int>(msg.Level);
                    p.Encode<int>(msg.Job);

                    await u.SendPacket(p);
                }));
            }, cancellationToken);
        }

        public override ISocketAdapter Build(ISocket socket)
            => new GameServiceAdapter(socket, this);

        public async Task TryTick()
        {
            await FieldManager.TryTick();
            await ContinentManager.TryTick();
        }
    }
}