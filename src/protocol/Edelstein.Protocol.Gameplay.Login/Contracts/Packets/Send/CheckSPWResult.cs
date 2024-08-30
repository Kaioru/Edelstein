using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record CheckSPWResult() : StructuredSendPacket((short)PacketSendOperation.CheckSPWResult)
{
    [FieldOrder(0)]
    public required LoginSPWResultCode Result { get; init; }
}
