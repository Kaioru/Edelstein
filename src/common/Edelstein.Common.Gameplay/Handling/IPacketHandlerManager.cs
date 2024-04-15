using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Buffers;

namespace Edelstein.Common.Gameplay.Handling;

public interface IPacketHandlerManager<TStageUser, TStageSystem> 
    where TStageUser : IStageUser<TStageUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageUser, TStageSystem>
{
    void Add(IPacketHandler<TStageUser, TStageSystem> handler);
    void Remove(IPacketHandler<TStageUser, TStageSystem> handler);

    Task Process(TStageUser user, IPacket packet);
}
