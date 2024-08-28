using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record CreateNewCharacter : StructuredRecvPacket
{
    [FieldOrder(0)] public required LPString Name { get; init; }
    [FieldOrder(1)] public int CurSelectedRace { get; init; }
    [FieldOrder(2)] public short CurSelectedSubJob { get; init; }
    
    [FieldOrder(3)] public int Face { get; init; }
    [FieldOrder(4)] public int Hair { get; init; }
    [FieldOrder(5)] public int HairColor { get; init; }
    [FieldOrder(6)] public int Skin { get; init; }
    [FieldOrder(7)] public int Coat { get; init; }
    [FieldOrder(8)] public int Pants { get; init; }
    [FieldOrder(9)] public int Shoes { get; init; }
    [FieldOrder(10)] public int Weapon { get; init; }
    
    [FieldOrder(11)] public byte Gender { get; init; }
}
