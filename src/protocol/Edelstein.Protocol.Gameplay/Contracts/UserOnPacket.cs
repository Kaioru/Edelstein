using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts;

public record UserOnPacket<TStageSystemUser, TStageSystem>(
    TStageSystemUser User,
    IRawPacket Packet
)
    where TStageSystemUser : IStageSystemUser<TStageSystemUser, TStageSystem>
    where TStageSystem : IStageSystem<TStageSystemUser, TStageSystem>;
