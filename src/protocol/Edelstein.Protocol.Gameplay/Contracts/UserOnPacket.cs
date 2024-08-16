using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacket<TStageSystem, TStageSystemUser>(
    TStageSystemUser User,
    IRawPacket Packet
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser>
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>;
