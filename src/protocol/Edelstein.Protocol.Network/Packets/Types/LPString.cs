using BinarySerialization;

namespace Edelstein.Protocol.Network.Packets.Types;

public record LPString(
    [property: FieldOrder(1)]
    [property: FieldLength(nameof(LPString.Length))]
    [property: SerializeAs(SerializedType.SizedString)]
    string Value
)
{
    [FieldOrder(0)] 
    [SerializeAs(SerializedType.Int2)]
    public int Length { get; init; } = Value.Length;
}
