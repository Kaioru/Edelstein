using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record EnableSPWRequest : StructuredRecvPacket
{
    [FieldOrder(0)] 
    public byte Unk1 { get; init; }
    
    [FieldOrder(1)] public required int CharacterID { get; init; }
    [FieldOrder(2)] public required LPString MacAddress { get; init; }
    [FieldOrder(3)] public required LPString MacAddressWithHDDSerial { get; init; }
    [FieldOrder(4)] public required LPString SPW { get; init; }
}
