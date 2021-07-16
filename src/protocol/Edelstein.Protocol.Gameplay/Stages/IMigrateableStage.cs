using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IMigrateableStage<TStage, TUser> : IPacketDispatcher, IRepositoryEntry<string>
        where TStage : IMigrateableStage<TStage, TUser>
        where TUser : IMigrateableStageUser<TStage, TUser>
    {
    }
}
