using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets.Types;

public record BPNullable<T>
{
    [FieldOrder(0)] public bool HasValue { get; init; }
    
    [FieldOrder(1)]
    [SerializeWhen(nameof(HasValue), true)]
    public T? Value { get; init; }
    
    public BPNullable() {}
    public BPNullable(T obj)
    {
        HasValue = true;
        Value = obj;
    }
}
