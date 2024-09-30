using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserItemOptionUpgradeItemUseRequest : StructuredRecvPacket, IItemUseInfo
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    [FieldOrder(1)] public short UPOS { get; init; }
    [FieldOrder(2)] public short EPOS { get; init; }
    [FieldOrder(3)] public bool EnchantSkill { get; init; }

    [Ignore] public short Pos => UPOS;
}
