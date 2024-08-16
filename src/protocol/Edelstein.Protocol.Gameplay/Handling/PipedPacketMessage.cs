using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Handling;

public record PipedPacketMessage<TStageSystem, TStageSystemUser, TPacket>(
    TStageSystemUser User,
    TPacket Packet
)
    where TStageSystem : IStageSystem<TStageSystem, TStageSystemUser> 
    where TStageSystemUser : IStageSystemUser<TStageSystem, TStageSystemUser>
    where TPacket : StructuredBasePacket;
