using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IMigrateableStage<TStage, TUser> : IPacketDispatcher, IRepositoryEntry<string>
        where TStage : IMigrateableStage<TStage, TUser>
        where TUser : IMigrateableStageUser<TStage, TUser>
    {
        IServerRegistryService ServerRegistryService { get; }
        ISessionRegistryService SessionRegistry { get; }
        IMigrationRegistryService MigrationRegistryService { get; }

        IAccountRepository AccountRepository { get; }
        IAccountWorldRepository AccountWorldRepository { get; }
        ICharacterRepository CharacterRepository { get; }
    }
}
