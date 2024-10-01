using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Items.Cash;

public record StructuredItemUnreleaseCashItemUseInfoEx : StructuredBasePacket, ICashItemUseInfoEx
{
    [FieldOrder(0)] public int EPOS { get; init; }
}
