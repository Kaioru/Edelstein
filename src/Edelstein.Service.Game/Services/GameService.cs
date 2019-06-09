using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Scripts;
using Edelstein.Core.Scripts.Lua;
using Edelstein.Core.Utils.Messaging;
using Edelstein.Database.Store;
using Edelstein.Network;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Scripted;
using Edelstein.Service.Game.Conversations.Speakers;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Fields.Continents;
using Edelstein.Service.Game.Services.Handlers;
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

        public CommandManager CommandManager { get; }
        public IConversationManager ConversationManager { get; }
        public FieldManager FieldManager { get; }
        public ContinentManager ContinentManager { get; }

        public IDictionary<RecvPacketOperations, IGameHandler> Handlers { get; }

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

            CommandManager = new CommandManager(new Parser(settings =>
            {
                settings.CaseSensitive = false;
                settings.CaseInsensitiveEnumValues = true;
            }));

            if (scriptManager is LuaScriptManager)
                Speakers.Types.ForEach(t => UserData.RegisterType(t));

            ConversationManager = new ScriptedConversationManager(scriptManager);
            FieldManager = new FieldManager(templateManager);
            ContinentManager = new ContinentManager(templateManager, FieldManager);

            Handlers = new Dictionary<RecvPacketOperations, IGameHandler>
            {
                [RecvPacketOperations.MigrateIn] = new MigrateInHandler(),
                [RecvPacketOperations.AliveAck] = new AliveAckHandler(),
                [RecvPacketOperations.FuncKeyMappedModified] = new FuncKeyMappedModifiedHandler(),
                [RecvPacketOperations.QuickslotKeyMappedModified] = new QuickSlotKeyMappedModifiedHandler(),

                [RecvPacketOperations.UserTransferFieldRequest] = new UserTransferFieldRequestHandler(),
                [RecvPacketOperations.UserTransferChannelRequest] = new UserTransferChannelRequestHandler(),
                [RecvPacketOperations.UserMigrateToCashShopRequest] = new UserMigrateToCashShopRequestHandler(),
                [RecvPacketOperations.UserMigrateToITCRequest] = new UserMigrateToITCRequestHandler(),
                [RecvPacketOperations.UserMove] = new UserMoveHandler(),
                [RecvPacketOperations.UserSitRequest] = new UserSitRequestHandler(),
                [RecvPacketOperations.UserPortableChairSitRequest] = new UserPortableChairSitRequestHandler(),
                [RecvPacketOperations.UserChat] = new UserChatHandler(),
                [RecvPacketOperations.UserEmotion] = new UserEmotionHandler(),
                [RecvPacketOperations.UserSelectNpc] = new UserSelectNPCHandler(),
                [RecvPacketOperations.UserScriptMessageAnswer] = new UserScriptMessageAnswerHandler(),
                [RecvPacketOperations.UserShopRequest] = new UserShopRequestHandler(),
                [RecvPacketOperations.UserTrunkRequest] = new UserTrunkRequestHandler(),
                [RecvPacketOperations.UserGatherItemRequest] = new UserGatherItemRequestHandler(),
                [RecvPacketOperations.UserSortItemRequest] = new UserSortItemRequestHandler(),
                [RecvPacketOperations.UserChangeSlotPositionRequest] = new UserChangeSlotPositionRequestHandler(),
                [RecvPacketOperations.UserStatChangeItemUseRequest] = new UserStatChangeItemUseRequestHandler(),
                [RecvPacketOperations.UserMobSummonItemUseRequest] = new UserMobSummonItemUseRequestHandler(),
                [RecvPacketOperations.UserPortalScrollUseRequest] = new UserPortalScrollUseRequestHandler(),
                [RecvPacketOperations.UserAbilityUpRequest] = new UserAbilityUpRequestHandler(),
                [RecvPacketOperations.UserAbilityMassUpRequest] = new UserAbilityMassUpRequestHandler(),
                [RecvPacketOperations.UserChangeStatRequest] = new UserChangeStatRequestHandler(),
                [RecvPacketOperations.UserSkillUpRequest] = new UserSkillUpRequestHandler(),
                [RecvPacketOperations.UserSkillUseRequest] = new UserSkillUseRequestHandler(),
                [RecvPacketOperations.UserSkillCancelRequest] = new UserSkillCancelRequestHandler(),
                [RecvPacketOperations.UserSkillPrepareRequest] = new UserSkillPrepareRequestHandler(),
                [RecvPacketOperations.UserCharacterInfoRequest] = new UserCharacterInfoRequestHandler(),
                [RecvPacketOperations.UserActivatePetRequest] = new UserActivatePetRequestHandler(),
                [RecvPacketOperations.UserPortalScriptRequest] = new UserPortalScriptRequestHandler(),
                [RecvPacketOperations.UserQuestRequest] = new UserQuestRequestHandler(),

                [RecvPacketOperations.PetMove] = new PetMoveHandler(),

                [RecvPacketOperations.SummonedMove] = new SummonedMoveHandler(),

                [RecvPacketOperations.DragonMove] = new DragonMoveHandler(),

                [RecvPacketOperations.MobMove] = new MobMoveHandler(),

                [RecvPacketOperations.NpcMove] = new NPCMoveHandler(),

                [RecvPacketOperations.DropPickUpRequest] = new DropPickupRequestHandler(),

                [RecvPacketOperations.CONTISTATE] = new ContiStateHandler()
            };

            var userAttackHandler = new UserAttackHandler();
            Handlers[RecvPacketOperations.UserMeleeAttack] = userAttackHandler;
            Handlers[RecvPacketOperations.UserShootAttack] = userAttackHandler;
            Handlers[RecvPacketOperations.UserMagicAttack] = userAttackHandler;
            Handlers[RecvPacketOperations.UserBodyAttack] = userAttackHandler;
        }

        public override async Task OnTick(DateTime now)
        {
            await base.OnTick(now);
            await FieldManager.OnTick(now);
            await ContinentManager.OnTick(now);
        }

        public override ISocket Build(IChannel channel, uint seqSend, uint seqRecv)
            => new GameSocket(channel, seqSend, seqRecv, this);
    }
}