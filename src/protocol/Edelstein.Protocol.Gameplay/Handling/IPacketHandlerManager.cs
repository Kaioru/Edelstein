using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public interface IPacketHandlerManager<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser> 
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
{
    void Add(IPacketHandler<TStageSystem, TStageSystemUser> handler);
    void Remove(IPacketHandler<TStageSystem, TStageSystemUser> handler);
    void Remove(short operation);

    Task Process(TStageSystemUser user, IRawPacket packet);
}
