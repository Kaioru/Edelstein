using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record EnableSPWResult() : StructuredSendPacket((short)PacketSendOperation.EnableSPWResult)
{
    [FieldOrder(0)]
    public bool Unk1 { get; init; }
    
    [FieldOrder(1)]
    public required LoginSPWResultCode Result { get; init; }
}
