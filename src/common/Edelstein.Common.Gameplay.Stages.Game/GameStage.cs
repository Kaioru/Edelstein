using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Admin;
using Edelstein.Common.Gameplay.Stages.Game.Commands.Common;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Handlers;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStage : AbstractServerStage<GameStage, GameStageUser, GameStageConfig>, IGameStage<GameStage, GameStageUser>
    {
        public int WorldID => Config.WorldID;
        public int ChannelID => Config.ChannelID;

        public ICommandProcessor CommandProcessor { get; }

        public ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        public ITemplateRepository<ItemOptionTemplate> ItemOptionTemplates { get; }
        public ITemplateRepository<ItemSetTemplate> ItemSetTemplates { get; }

        public ITemplateRepository<FieldTemplate> FieldTemplates { get; }
        public ITemplateRepository<NPCTemplate> NPCTemplates { get; }

        public IFieldRepository FieldRepository { get; }
        public IFieldSetRepository FieldSetRepository { get; }
        public IContiMoveRepository ContiMoveRepository { get; }

        public GameStage(
            GameStageConfig config,
            ILogger<IStage<GameStage, GameStageUser>> logger,
            IServerRegistryService serverRegistryService,
            ISessionRegistryService sessionRegistry,
            IMigrationRegistryService migrationRegistryService,
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
            ITemplateRepository<NPCTemplate> npcTemplates
        ) : base(
            ServerStageType.Game,
            config,
            logger,
            serverRegistryService,
            sessionRegistry,
            migrationRegistryService,
            accountRepository,
            accountWorldRepository,
            characterRepository,
            tickerManager,
            packetProcessor
        )
        {
            CommandProcessor = commandProcessor;
            ItemTemplates = itemTemplates;
            ItemOptionTemplates = itemOptionTemplates;
            ItemSetTemplates = itemSetTemplates;
            FieldTemplates = fieldTemplates;
            NPCTemplates = npcTemplates;

            FieldRepository = new FieldRepository(this, FieldTemplates, tickerManager);
            FieldSetRepository = null; // TODO;
            ContiMoveRepository = null;

            packetProcessor.Register(new UserTransferChannelRequestHandler());
            packetProcessor.Register(new UserMoveHandler());
            packetProcessor.Register(new UserEmotionHandler());
            packetProcessor.Register(new UserChatHandler());
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

            user.FieldUser = fieldUser;
            await field.Enter(fieldUser);
        }

        public override async Task Leave(GameStageUser user)
        {
            if (user.Field != null)
                await user.Field.Leave(user.FieldUser);

            await base.Leave(user);
        }
    }

}
