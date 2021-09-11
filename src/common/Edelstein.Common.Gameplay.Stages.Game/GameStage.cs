using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Admin;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Common;
using Edelstein.Common.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Common.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Services.Contracts.Social;
using Edelstein.Protocol.Services.Social;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Edelstein.Common.Gameplay.Stages.Game.Continent;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Handlers;
using Edelstein.Common.Gameplay.Users.Skills.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStage : AbstractServerStage<GameStage, GameStageUser, GameStageConfig>, IGameStage<GameStage, GameStageUser>
    {
        public int WorldID => Info.WorldID;
        public int ChannelID => Info.ChannelID;

        public IDispatchService DispatchService { get; }

        public IInviteService InviteService { get; }
        public IGuildService GuildService { get; }
        public IPartyService PartyService { get; }

        public ICommandProcessor CommandProcessor { get; }

        public ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        public ITemplateRepository<ItemStringTemplate> ItemStringTemplates { get; }
        public ITemplateRepository<ItemOptionTemplate> ItemOptionTemplates { get; }
        public ITemplateRepository<ItemSetTemplate> ItemSetTemplates { get; }

        public ITemplateRepository<CharacterSkillTemplate> CharacterSkillTemplates { get; }

        public ITemplateRepository<FieldTemplate> FieldTemplates { get; }
        public ITemplateRepository<FieldStringTemplate> FieldStringTemplates { get; }
        public ITemplateRepository<ContiMoveTemplate> ContiMoveTemplates { get; }
        public ITemplateRepository<NPCTemplate> NPCTemplates { get; }
        public ITemplateRepository<MobTemplate> MobTemplates { get; }

        public IFieldRepository FieldRepository { get; }
        public IFieldSetRepository FieldSetRepository { get; }
        public IContiMoveRepository ContiMoveRepository { get; }

        public GameStage(
            GameStageConfig config,
            ILogger<IStage<GameStage, GameStageUser>> logger,
            IServerRegistry serverRegistry,
            ISessionRegistry sessionRegistry,
            IMigrationRegistry migrationRegistry,
            IDispatchService dispatchService,
            IInviteService inviteService,
            IGuildService guildService,
            IPartyService partyService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager tickerManager,
            IPacketProcessor<GameStage, GameStageUser> packetProcessor,
            ICommandProcessor commandProcessor,
            ITemplateRepository<ItemTemplate> itemTemplates,
            ITemplateRepository<ItemStringTemplate> itemStringTemplates,
            ITemplateRepository<ItemOptionTemplate> itemOptionTemplates,
            ITemplateRepository<ItemSetTemplate> itemSetTemplates,
            ITemplateRepository<CharacterSkillTemplate> characterSkillTemplates,
            ITemplateRepository<FieldTemplate> fieldTemplates,
            ITemplateRepository<FieldStringTemplate> fieldStringTemplates,
            ITemplateRepository<ContiMoveTemplate> contiMoveTemplates,
            ITemplateRepository<NPCTemplate> npcTemplates,
            ITemplateRepository<MobTemplate> mobTemplates
        ) : base(
            ServerStageType.Game,
            config,
            logger,
            serverRegistry,
            sessionRegistry,
            migrationRegistry,
            accountRepository,
            accountWorldRepository,
            characterRepository,
            tickerManager,
            packetProcessor
        )
        {
            DispatchService = dispatchService;

            dispatchService
                .Subscribe(new DispatchSubscription { Server = ID })
                .ForEachAwaitAsync(OnDispatch);

            InviteService = inviteService;
            GuildService = guildService;
            PartyService = partyService;

            guildService
                .Subscribe()
                .ForEachAwaitAsync(OnGuildUpdate);
            partyService
                .Subscribe()
                .ForEachAwaitAsync(OnPartyUpdate);

            CommandProcessor = commandProcessor;
            ItemTemplates = itemTemplates;
            ItemStringTemplates = itemStringTemplates;
            ItemOptionTemplates = itemOptionTemplates;
            ItemSetTemplates = itemSetTemplates;
            CharacterSkillTemplates = characterSkillTemplates;
            FieldTemplates = fieldTemplates;
            FieldStringTemplates = fieldStringTemplates;
            ContiMoveTemplates = contiMoveTemplates;
            NPCTemplates = npcTemplates;
            MobTemplates = mobTemplates;

            FieldRepository = new FieldRepository(this, FieldTemplates, tickerManager);
            FieldSetRepository = null; // TODO;
            ContiMoveRepository = new ContiMoveRepository(this, ContiMoveTemplates, FieldRepository, tickerManager);

            packetProcessor.Register(new UserTransferChannelRequestHandler());
            packetProcessor.Register(new UserMoveHandler());
            packetProcessor.Register(new UserEmotionHandler());
            packetProcessor.Register(new UserChatHandler());
            packetProcessor.Register(new UserScriptMessageAnswerHandler());
            packetProcessor.Register(new UserGatherItemRequestHandler());
            packetProcessor.Register(new UserSortItemRequestHandler());
            packetProcessor.Register(new UserChangeSlotPositionRequestHandler());
            packetProcessor.Register(new UserCharacterInfoRequestHandler());

            packetProcessor.Register(new GroupMessageHandler());
            packetProcessor.Register(new WhisperHandler());

            packetProcessor.Register(new PartyRequestHandler());
            packetProcessor.Register(new PartyResultHandler());

            packetProcessor.Register(new MobMoveHandler());

            packetProcessor.Register(new NPCMoveHandler());

            packetProcessor.Register(new ContiStateHandler());

            commandProcessor.Register(new HelpCommand(commandProcessor));
            commandProcessor.Register(new AliasCommand(commandProcessor));
            commandProcessor.Register(new DebugCommand());

            commandProcessor.Register(new StatCommand());
            commandProcessor.Register(new ItemCommand(ItemStringTemplates, ItemTemplates));
            commandProcessor.Register(new FieldCommand(FieldRepository, FieldStringTemplates, FieldTemplates));
            commandProcessor.Register(new ContiMoveCommand(ContiMoveRepository));
        }

        public override async Task Enter(GameStageUser user)
        {
            await base.Enter(user);

            var field = await FieldRepository.Retrieve(user.Character.FieldID);
            var fieldUser = new FieldObjUser(user);

            var guildLoadResponse = await GuildService.LoadByCharacter(new GuildLoadByCharacterRequest { Character = user.Character.ID });
            var partyLoadResponse = await PartyService.LoadByCharacter(new PartyLoadByCharacterRequest { Character = user.Character.ID });

            if (guildLoadResponse.Guild != null) fieldUser.Guild = new Guild(guildLoadResponse.Guild);
            if (partyLoadResponse.Party != null) fieldUser.Party = new Party(partyLoadResponse.Party);

            user.FieldUser = fieldUser;

            await field.Enter(fieldUser);

            var functionKeyPacket = new UnstructuredOutgoingPacket(PacketSendOperations.FuncKeyMappedInit);
            var quickSlotKeyPacket = new UnstructuredOutgoingPacket(PacketSendOperations.QuickslotMappedInit);

            functionKeyPacket.WriteBool(false);

            for (var i = 0; i < 90; i++)
            {
                var key = user.Character.FunctionKeys[i];

                functionKeyPacket.WriteByte(key?.Type ?? 0);
                functionKeyPacket.WriteInt(key?.Action ?? 0);
            }

            quickSlotKeyPacket.WriteBool(false);

            for (var i = 0; i < 8; i++)
                quickSlotKeyPacket.WriteInt(user.Character.QuickSlotKeys[i].Key);

            _ = user.Dispatch(functionKeyPacket);
            _ = user.Dispatch(quickSlotKeyPacket);
        }

        public override async Task Leave(GameStageUser user)
        {
            if (user.Field != null)
                await user.Field.Leave(user.FieldUser);

            await base.Leave(user);
        }

        private async Task OnDispatch(DispatchContract contract)
        {
            var targets = GetUsers();
            var packet = new UnstructuredOutgoingPacket();

            packet.WriteBytes(contract.Data.ToArray());

            if (contract.TargetCharacters.Count > 0)
            {
                targets = targets
                    .Where(u => contract.TargetCharacters.Contains(u.ID))
                    .ToList();
            }

            await Task.WhenAll(targets.Select(t => t.Dispatch(packet)));
        }

        private Task OnGuildUpdate(GuildUpdateContract contract)
        {
            var guild = new Guild(contract.Guild);
            var memberIDs = guild.Members
                .Select(m => m.ID)
                .ToImmutableList();
            var users = GetUsers()
                .Where(u => u.FieldUser != null)
                .Select(u => u.FieldUser)
                .Where(u => u.Guild?.ID == guild.ID || memberIDs.Contains(u.ID))
                .ToImmutableList();

            users.ForEach(u => u.Guild = memberIDs.Contains(u.ID) ? guild : null);
            return Task.CompletedTask;
        }

        private Task OnPartyUpdate(PartyUpdateContract contract)
        {
            var party = new Party(contract.Party);
            var memberIDs = party.Members
                .Select(m => m.ID)
                .ToImmutableList();
            var users = GetUsers()
                .Where(u => u.FieldUser != null)
                .Select(u => u.FieldUser)
                .Where(u => u.Party?.ID == party.ID || memberIDs.Contains(u.ID))
                .ToImmutableList();

            users.ForEach(u => u.Party = memberIDs.Contains(u.ID) ? party : null);
            return Task.CompletedTask;
        }
    }
}