using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Gameplay.Handling;

namespace Edelstein.Common.Gameplay.Handling;

public interface IPacketHandlerManagerEntry<TStageSystem, in TStageSystemUser> :
    IPacketHandler<TStageSystem, TStageSystemUser>
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
{
    public short Operation { get; }
}
