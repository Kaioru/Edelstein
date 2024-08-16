using System.Threading.Tasks;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public interface IPacketHandler<TStageSystem, in TStageSystemUser> 
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser> 
{
    short Operation { get; }
    
    Task Handle(TStageSystemUser user, IRawPacket packet);
}
