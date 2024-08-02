using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets;

public record LoginAccountInfoFrame : StructuredBasePacket
{
    [FieldOrder(0)] public required int ID { get; init; }
    [FieldOrder(1)] public byte Gender { get; init; }
    [FieldOrder(2)] public byte GradeCode { get; init; }
    [FieldOrder(3)] public short SubGradeCode { get; init; }
    [FieldOrder(4)] public byte CountryID { get; init; }
    [FieldOrder(5)] public required LPString NexonClubID { get; init; }
    
    [FieldOrder(6)] public byte Unk1 { get; init; }
    [FieldOrder(7)] public byte Unk2 { get; init; }
    [FieldOrder(8)] public FDateTime ChatUnblockDate { get; init; } = new();
    [FieldOrder(9)] public FDateTime RegisterDate { get; init; } = new();

    [FieldOrder(10)] public int NumOfCharacter { get; init; } = 4;
    
    [FieldOrder(11)] public bool Unk3 { get; init; } = true;
    [FieldOrder(12)] public bool Unk4 { get; init; }
    [FieldOrder(13)] public long ClientKey { get; init; }
}
