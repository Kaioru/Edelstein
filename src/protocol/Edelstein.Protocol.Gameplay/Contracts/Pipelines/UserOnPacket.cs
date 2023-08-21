using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts.Pipelines;

public record UserOnPacket<TStageUser>(
    TStageUser User,
    IPacket Packet
) where TStageUser : IStageUser<TStageUser>;
