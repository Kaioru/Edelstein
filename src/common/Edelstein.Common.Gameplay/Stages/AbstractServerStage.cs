using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Behaviors;
using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractServerStage<TStage, TUser, TConfig> : AbstractStage<TStage, TUser>, IServerStage<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageConfig
    {
        private static readonly TimeSpan ServerUpdateFreq = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan AliveBehaviorFreq = TimeSpan.FromSeconds(1);

        public ServerStageType Type { get; init; }
        public TConfig Config { get; init; }
        public string ID => Config.ID;

        public ILogger Logger { get; init; }

        public IServerRegistry ServerRegistry { get; init; }
        public ISessionRegistry SessionRegistry { get; init; }
        public IMigrationRegistry MigrationRegistry { get; init; }

        public IAccountRepository AccountRepository { get; init; }
        public IAccountWorldRepository AccountWorldRepository { get; init; }
        public ICharacterRepository CharacterRepository { get; init; }

        protected AbstractServerStage(
            ServerStageType type,
            TConfig config,
            ILogger<IStage<TStage, TUser>> logger,
            IServerRegistry serverRegistry,
            ISessionRegistry sessionRegistry,
            IMigrationRegistry migrationRegistry,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager tickerManager,
            IPacketProcessor<TStage, TUser> processor
        ) : base()
        {
            Type = type;
            Config = config;
            Logger = logger;
            ServerRegistry = serverRegistry;
            SessionRegistry = sessionRegistry;
            MigrationRegistry = migrationRegistry;
            AccountRepository = accountRepository;
            AccountWorldRepository = accountWorldRepository;
            CharacterRepository = characterRepository;

            tickerManager.Schedule(new ServerUpdateBehavior<TStage, TUser, TConfig>((TStage)this), ServerUpdateFreq, TimeSpan.Zero);
            tickerManager.Schedule(new AliveReqBehavior<TStage, TUser, TConfig>((TStage)this), AliveBehaviorFreq);
            processor.Register(new AliveAckHandler<TStage, TUser, TConfig>());
            processor.Register(new MigrateInHandler<TStage, TUser, TConfig>((TStage)this));
            processor.Register(new ClientDumpLogHandler<TStage, TUser, TConfig>((TStage)this));
        }

        public override async Task Enter(TUser user)
        {
            var session = new SessionContract
            {
                Server = ID,
                State = SessionState.LoggedIn
            };

            if (user.Account != null) session.Account = user.Account.ID;
            if (user.Character != null) session.Character = user.Character.ID;

            var result = user.IsLoggingIn
                ? (await SessionRegistry.Start(new StartSessionRequest { Session = session })).Result
                : (await SessionRegistry.Update(new UpdateSessionRequest { Session = session })).Result;

            if (result != SessionRegistryResult.Ok)
            {
                await user.Disconnect();
                return;
            }

            await base.Enter(user);
        }

        public override async Task Leave(TUser user)
        {
            if (user.Account != null && !user.IsMigrating)
            {
                var session = new SessionContract
                {
                    Account = user.Account.ID,
                    Character = user.Character?.ID,
                    State = SessionState.Offline
                };

                await SessionRegistry.Update(new UpdateSessionRequest { Session = session });
            }
            await base.Leave(user);
        }
    }
}
