using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Gameplay.Handling;

public interface IPacketSerializer<in TStageUser, TStageSystem, out TObject> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    TObject? Serialize(TStageUser user, IPacketReader reader);
}
