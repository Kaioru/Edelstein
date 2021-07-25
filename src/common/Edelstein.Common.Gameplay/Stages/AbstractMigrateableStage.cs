using System;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Behaviors;
using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractMigrateableStage<TStage, TUser> : AbstractStage<TStage, TUser>, IMigrateableStage<TStage, TUser>
        where TStage : AbstractMigrateableStage<TStage, TUser>
        where TUser : AbstractMigrateableStageUser<TStage, TUser>
    {
        public static TimeSpan AliveBehaviorFreq = TimeSpan.FromSeconds(1);

        public abstract string ID { get; }

        public IServerRegistryService ServerRegistryService { get; init; }
        public ISessionRegistryService SessionRegistry { get; init; }
        public IMigrationRegistryService MigrationRegistryService { get; init; }

        public IAccountRepository AccountRepository { get; init; }
        public IAccountWorldRepository AccountWorldRepository { get; init; }
        public ICharacterRepository CharacterRepository { get; init; }

        protected AbstractMigrateableStage(
            IServerRegistryService serverRegistryService,
            ISessionRegistryService sessionRegistry,
            IMigrationRegistryService migrationRegistryService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager timerManager,
            IPacketProcessor<TStage, TUser> processor
        ) : base(processor)
        {
            ServerRegistryService = serverRegistryService;
            SessionRegistry = sessionRegistry;
            MigrationRegistryService = migrationRegistryService;
            AccountRepository = accountRepository;
            AccountWorldRepository = accountWorldRepository;
            CharacterRepository = characterRepository;

            timerManager.Schedule(new AliveReqBehavior<TStage, TUser>((TStage)this), AliveBehaviorFreq);
            processor.Register(new AliveAckHandler<TStage, TUser>());
            processor.Register(new MigrateInHandler<TStage, TUser>((TStage)this));
        }
    }
}
