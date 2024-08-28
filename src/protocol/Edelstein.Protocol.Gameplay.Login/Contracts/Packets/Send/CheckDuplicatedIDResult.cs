using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record CheckDuplicatedIDResult() : StructuredSendPacket((short)PacketSendOperation.CheckDuplicatedIDResult)
{
    [FieldOrder(0)]
    public required LPString CheckedName { get; init; }
    
    [FieldOrder(1)]
    public byte Reason { get; init; }
}
