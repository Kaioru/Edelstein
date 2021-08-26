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
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
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

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStage : AbstractServerStage<GameStage, GameStageUser, GameStageConfig>, IGameStage<GameStage, GameStageUser>
    {
        public int WorldID => Config.WorldID;
        public int ChannelID => Config.ChannelID;

        public IDispatchService DispatchService { get; }
        public IGuildService GuildService { get; }
        public IPartyService PartyService { get; }

        public ICommandProcessor CommandProcessor { get; }

        public ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        public ITemplateRepository<ItemOptionTemplate> ItemOptionTemplates { get; }
        public ITemplateRepository<ItemSetTemplate> ItemSetTemplates { get; }

        public ITemplateRepository<FieldTemplate> FieldTemplates { get; }
        public ITemplateRepository<ContiMoveTemplate> ContiMoveTemplates { get; }
        public ITemplateRepository<NPCTemplate> NPCTemplates { get; }

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
            IGuildService guildService,
            IPartyService partyService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager tickerManager,
            IPacketProcessor<GameStage, GameStageUser> packetProcessor,
            ICommandProcessor commandProcessor,
            ITemplateRepository<ItemTemplate> itemTemplates,
            ITemplateRepository<ItemOptionTemplate> itemOptionTemplates,
            ITemplateRepository<ItemSetTemplate> itemSetTemplates,
            ITemplateRepository<FieldTemplate> fieldTemplates,
            ITemplateRepository<ContiMoveTemplate> contiMoveTemplates,
            ITemplateRepository<NPCTemplate> npcTemplates
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
            GuildService = guildService;
            PartyService = partyService;

            dispatchService
                .Subscribe(new DispatchSubscription { Server = ID })
                .ForEachAwaitAsync(OnDispatch);
            guildService
                .Subscribe()
                .ForEachAwaitAsync(OnGuildUpdate);
            partyService
                .Subscribe()
                .ForEachAwaitAsync(OnPartyUpdate);

            CommandProcessor = commandProcessor;
            ItemTemplates = itemTemplates;
            ItemOptionTemplates = itemOptionTemplates;
            ItemSetTemplates = itemSetTemplates;
            FieldTemplates = fieldTemplates;
            ContiMoveTemplates = contiMoveTemplates;
            NPCTemplates = npcTemplates;

            FieldRepository = new FieldRepository(this, FieldTemplates, tickerManager);
            FieldSetRepository = null; // TODO;
            ContiMoveRepository = null;

            packetProcessor.Register(new UserTransferChannelRequestHandler());
            packetProcessor.Register(new UserMoveHandler());
            packetProcessor.Register(new UserEmotionHandler());
            packetProcessor.Register(new UserChatHandler());
            packetProcessor.Register(new UserScriptMessageAnswerHandler());
            packetProcessor.Register(new UserGatherItemRequestHandler());
            packetProcessor.Register(new UserSortItemRequestHandler());
            packetProcessor.Register(new UserChangeSlotPositionRequestHandler());

            packetProcessor.Register(new NPCMoveHandler());

            commandProcessor.Register(new HelpCommand(commandProcessor));
            commandProcessor.Register(new AliasCommand(commandProcessor));
            commandProcessor.Register(new DebugCommand());

            commandProcessor.Register(new StatCommand());
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

            if (fieldUser.Guild != null)
            {
                var guildPacket = new UnstructuredOutgoingPacket(PacketSendOperations.GuildResult);

                guildPacket.WriteByte((byte)GuildResultCode.LoadGuild_Done);
                guildPacket.WriteBool(true);
                guildPacket.WriteGuildData(fieldUser.Guild);
                await user.Dispatch(guildPacket);
            }

            if (fieldUser.Party != null)
            {
                var partyPacket = new UnstructuredOutgoingPacket(PacketSendOperations.PartyResult);

                partyPacket.WriteByte((byte)PartyResultCode.LoadParty_Done);
                partyPacket.WriteInt(fieldUser.Party.ID);
                partyPacket.WritePartyData(fieldUser.Party, ChannelID);
                await user.Dispatch(partyPacket);
            }

            user.FieldUser = fieldUser;

            await field.Enter(fieldUser);
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

        private async Task OnGuildUpdate(GuildUpdateContract contract)
        {
        }

        private async Task OnPartyUpdate(PartyUpdateContract contract)
        {
        }
    }
}