using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities.Inventories;

public record StructuredItemSlot : StructuredBasePacket
{
    [FieldOrder(0)]
    public required ItemSlotType Type { get; init; }
    
    [FieldOrder(1)]
    [Subtype(nameof(Type), ItemSlotType.Equip, typeof(StructuredItemSlotInfoEquip))]
    [Subtype(nameof(Type), ItemSlotType.Bundle, typeof(StructuredItemSlotInfoBundle))]
    [Subtype(nameof(Type), ItemSlotType.Pet, typeof(StructuredItemSlotInfoPet))]
    [SubtypeDefault(typeof(StructuredItemSlotInfoBase))]
    public required StructuredItemSlotInfoBase Info { get; init; }
}
