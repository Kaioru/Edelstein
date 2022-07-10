using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IServerStage<TStage, TUser> : IStage<TStage, TUser>, IRepositoryEntry<string>
        where TStage : IServerStage<TStage, TUser>
        where TUser : IServerStageUser<TStage, TUser>
    {
        IServerRegistry ServerRegistry { get; }
        ISessionRegistry SessionRegistry { get; }
        IMigrationRegistry MigrationRegistry { get; }

        IAccountRepository AccountRepository { get; }
        IAccountWorldRepository AccountWorldRepository { get; }
        ICharacterRepository CharacterRepository { get; }
    }
}
