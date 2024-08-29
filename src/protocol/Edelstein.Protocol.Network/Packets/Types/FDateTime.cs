using System;
using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets.Types;

public record FDateTime(
    [property: Ignore] 
    DateTime Value 
)
{
    public FDateTime() : this(DateTime.FromFileTime(0)) {}
    public FDateTime(long value) : this(DateTime.FromFileTime(0)) => Data = value;

    [FieldOrder(0)] public long Data { get; init; } = Value.ToFileTime();
}
