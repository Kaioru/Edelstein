using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserEnterField() : StructuredSendPacket((short)PacketSendOperation.UserEnterField)
{
    [FieldOrder(0)]
    public required int ObjectID { get; init; }
}
