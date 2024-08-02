using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public interface IPacketHandlerManager<TStageSystemUser, TStageSystem> 
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
{
    void Add(IPacketHandler<TStageSystemUser, TStageSystem> handler);
    void Remove(IPacketHandler<TStageSystemUser, TStageSystem> handler);
    void Remove(short operation);

    Task Process(TStageSystemUser user, IRawPacket packet);
}
