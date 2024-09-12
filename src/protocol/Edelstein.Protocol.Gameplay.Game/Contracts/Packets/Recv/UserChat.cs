using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserChat : StructuredRecvPacket
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    
    [FieldOrder(1)] public required LPString Text { get; init; }
    [FieldOrder(2)] public bool OnlyBalloon { get; init; }
}
