using BinarySerialization;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Items.Cash;

public record StructuredAdBoardCashItemUseInfoEx : ICashItemUseInfoEx
{
    [FieldOrder(0)] public required LPString Text { get; init; }
}
