using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets.Types;

public record LPString(string Value)
{
    [FieldOrder(0)] 
    [SerializeAs(SerializedType.Int2)]
    public int Length { get; } = Value.Length;

    [FieldOrder(1)] 
    [FieldLength(nameof(Length))]
    [SerializeAs(SerializedType.SizedString)]
    public string Value { get; } = Value;
}
