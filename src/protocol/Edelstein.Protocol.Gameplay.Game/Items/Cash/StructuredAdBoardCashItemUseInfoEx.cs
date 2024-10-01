using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Items.Cash;

public record StructuredAdBoardCashItemUseInfoEx : StructuredBasePacket, ICashItemUseInfoEx
{
    [FieldOrder(0)] public required LPString Text { get; init; }
}
