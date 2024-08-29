using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterStat : StructuredBasePacket
{
    [FieldOrder(1)] public required int ID { get; init; }
    
    [FieldOrder(2)]
    [FieldLength(0xD)]
    public required string Name { get; init; }
    
    [FieldOrder(3)] public byte Gender { get; init; }
    [FieldOrder(4)] public byte Skin { get; init; }
    [FieldOrder(5)] public int Face { get; init; }
    [FieldOrder(6)] public int Hair { get; init; }
    
    [FieldOrder(7)] 
    [FieldCount(0x3)]
    public long[] PetLockerSN { get; init; } = {0, 0, 0};
    
    [FieldOrder(8)] public byte Level { get; init; }
    [FieldOrder(9)] public short Job { get; init; }
    [FieldOrder(10)] public short STR { get; init; }
    [FieldOrder(11)] public short DEX { get; init; }
    [FieldOrder(12)] public short INT { get; init; }
    [FieldOrder(13)] public short LUK { get; init; }
    [FieldOrder(14)] public int HP { get; init; }
    [FieldOrder(15)] public int MaxHP { get; init; }
    [FieldOrder(16)] public int MP { get; init; }
    [FieldOrder(17)] public int MaxMP { get; init; }
    
    [FieldOrder(18)] public short AP { get; init; }
    [FieldOrder(19)] public short SP { get; init; }
    
    [FieldOrder(20)] public int EXP { get; init; }
    [FieldOrder(21)] public short POP { get; init; }
    [FieldOrder(22)] public int TempEXP { get; init; }
    
    [FieldOrder(23)] public int PosMap { get; init; }
    [FieldOrder(24)] public byte Portal { get; init; }
    
    [FieldOrder(25)] public int Playtime { get; init; }
    
    [FieldOrder(26)] public short SubJob { get; init; }
}
