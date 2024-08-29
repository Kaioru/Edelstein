using BinarySerialization;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlotInfoPet : StructuredItemSlotInfoBase
{
    [FieldOrder(0)]
    [FieldLength(0xD)]
    public required string PetName { get; init; }
    
    [FieldOrder(1)] public short PetAttribute { get; init; }
    [FieldOrder(2)] public short PetSkill { get; init; }

    [FieldOrder(3)] public byte Level { get; init; }
    [FieldOrder(4)] public short Tameness { get; init; }
    [FieldOrder(5)] public byte Repleteness { get; init; }

    [FieldOrder(6)] public FDateTime DateDead { get; init; } = new();

    [FieldOrder(7)] public int RemainLife { get; init; }

    [FieldOrder(8)] public short Attribute { get; init; }
}
