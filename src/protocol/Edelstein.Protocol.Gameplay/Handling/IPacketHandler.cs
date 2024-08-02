using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public interface IPacketHandler<in TStageSystemUser, TStageSystem> 
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem> 
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>
{
    short Operation { get; }
    
    Task Handle(TStageSystemUser user, IRawPacket packet);
}
