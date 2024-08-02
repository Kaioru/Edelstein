using System;
using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets.Types;

public record FDateTime(
    [property: Ignore] 
    DateTime Value 
)
{
    public FDateTime() : this(DateTime.FromFileTime(0)) {}
    
    [FieldOrder(0)] public long Data { get; init; } = Value.ToFileTime();
}
