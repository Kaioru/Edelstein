using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Login.Handlers;
using Edelstein.Common.Gameplay.Stages.Login.Templates;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Common.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Common.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginStage : AbstractServerStage<LoginStage, LoginStageUser, LoginStageConfig>, ILoginStage<LoginStage, LoginStageUser>
    {
        public IGuildService GuildService { get; }
        public IPartyService PartyService { get; }

        public ITemplateRepository<WorldTemplate> WorldTemplates { get; }
        public ITemplateRepository<ItemTemplate> ItemTemplates { get; }

        public LoginStage(
            LoginStageConfig config,
            ILogger<IStage<LoginStage, LoginStageUser>> logger,
            IServerRegistry serverRegistry,
            ISessionRegistry sessionRegistry,
            IMigrationRegistry migrationRegistry,
            IGuildService guildService,
            IPartyService partyService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager timerManager,
            IPacketProcessor<LoginStage, LoginStageUser> processor,
            ITemplateRepository<WorldTemplate> worldTemplates,
            ITemplateRepository<ItemTemplate> itemTemplates
        ) : base(
            ServerStageType.Login,
            config,
            logger,
            serverRegistry,
            sessionRegistry,
            migrationRegistry,
            accountRepository,
            accountWorldRepository,
            characterRepository,
            timerManager,
            processor
        )
        {
            Logger = logger;

            GuildService = guildService;
            PartyService = partyService;

            WorldTemplates = worldTemplates;
            ItemTemplates = itemTemplates;

            processor.Register(new CheckPasswordHandler(this));
            processor.Register(new WorldInfoRequestHandler());
            processor.Register(new SelectWorldHandler());
            processor.Register(new CheckUserLimitHandler());
            processor.Register(new SetGenderHandler());
            processor.Register(new CheckPinCodeHandler());
            processor.Register(new WorldRequestHandler());
            processor.Register(new LogoutWorldHandler());
            processor.Register(new CheckDuplicatedIDHandler());
            processor.Register(new CreateNewCharacterHandler());
            processor.Register(new DeleteCharacterHandler());
            processor.Register(new ExceptionLogHandler(this));
            processor.Register(new EnableSPWRequestHandler(false));
            processor.Register(new CheckSPWRequestHandler(false));
        }
    }
}
