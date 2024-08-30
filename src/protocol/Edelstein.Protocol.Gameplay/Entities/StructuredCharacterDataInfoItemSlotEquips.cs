using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterDataInfoItemSlotEquips : StructuredBasePacket
{
    [FieldOrder(0)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotEquip> Equipped { get; init; }
    
    [FieldOrder(1)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotEquip> Equipped2 { get; init; }
    
    [FieldOrder(2)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotEquip> Equip { get; init; }
    
    [FieldOrder(3)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotEquip> Dragon { get; init; }
    
    [FieldOrder(4)]
    [SerializeUntil((short)0)]
    public required List<StructuredCharacterDataInfoItemSlotEquip> Mechanic { get; init; }
}

public record StructuredCharacterDataInfoItemSlotEquip : StructuredBasePacket
{
    [FieldOrder(0)]
    public required short Slot { get; init; }
    
    [FieldOrder(1)]
    public required StructuredItemSlot Item { get; init; }
}
