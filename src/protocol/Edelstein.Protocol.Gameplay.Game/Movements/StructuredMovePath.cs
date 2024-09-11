using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Movements;

public record StructuredMovePath : StructuredBasePacket
{
    [FieldOrder(0)] public short X { get; init; }
    [FieldOrder(1)] public short Y { get; init; }
    
    [FieldOrder(2)] public short VX { get; init; }
    [FieldOrder(3)] public short VY { get; init; }
    
    [FieldOrder(4)] public byte Count { get; init; }
    
    [FieldOrder(5)] 
    [FieldCount(nameof(Count))]
    public List<StructuredMovePathFragment> Fragments { get; init; } = new();
}
