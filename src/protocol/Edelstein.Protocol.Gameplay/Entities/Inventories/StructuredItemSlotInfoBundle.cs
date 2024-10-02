using BinarySerialization;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlotInfoBundle : StructuredItemSlotInfoBase
{
    [FieldOrder(0)] public short Number { get; init; }
    [FieldOrder(1)] public LPString Title { get; init; } = new();
    [FieldOrder(2)] public short Attribute { get; init; }

    [FieldOrder(3)] 
    public long? SN { get; init; }
}
