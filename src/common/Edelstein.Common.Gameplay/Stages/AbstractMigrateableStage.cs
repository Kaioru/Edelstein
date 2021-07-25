using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractMigrateableStage<TStage, TUser> : AbstractStage<TStage, TUser>, IMigrateableStage<TStage, TUser>
        where TStage : AbstractMigrateableStage<TStage, TUser>
        where TUser : AbstractMigrateableStageUser<TStage, TUser>
    {
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
            IPacketProcessor<TStage, TUser> processor
        ) : base(processor)
        {
            ServerRegistryService = serverRegistryService;
            SessionRegistry = sessionRegistry;
            MigrationRegistryService = migrationRegistryService;
            AccountRepository = accountRepository;
            AccountWorldRepository = accountWorldRepository;
            CharacterRepository = characterRepository;
        }
    }
}
