using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterDataInfoInventorySize : StructuredBasePacket
{
    [FieldOrder(0)]
    public required byte Equip { get; set; }
    
    [FieldOrder(1)]
    public required byte Consume { get; set; }
    
    [FieldOrder(2)]
    public required byte Install { get; set; }

    [FieldOrder(3)]
    public required byte Etc { get; set; }

    [FieldOrder(4)]
    public required byte Cash { get; set; }
}
