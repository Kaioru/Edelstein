using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterDataInfoItemSlotBundles : StructuredBasePacket
{
    [FieldOrder(0)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotBundle> Items { get; init; }
}

public record StructuredCharacterDataInfoItemSlotBundle : StructuredBasePacket
{
    [FieldOrder(0)]
    public required byte Slot { get; init; }
    
    [FieldOrder(1)]
    public required StructuredItemSlot Item { get; init; }
}
