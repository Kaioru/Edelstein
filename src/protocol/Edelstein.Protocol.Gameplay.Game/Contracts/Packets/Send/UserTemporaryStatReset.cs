using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserTemporaryStatReset() : StructuredSendPacket((short)PacketSendOperation.UserTemporaryStatReset)
{
    [FieldOrder(0)]
    public required int ObjectID { get; init; }

    [FieldOrder(1)]
    [FieldCount(4)]
    public required int[] Flag { get; init; }
}
