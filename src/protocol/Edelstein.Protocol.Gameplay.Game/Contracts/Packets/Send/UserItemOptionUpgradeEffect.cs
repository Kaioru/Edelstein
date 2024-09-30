using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserItemOptionUpgradeEffect() : StructuredSendPacket((short)PacketSendOperation.UserItemOptionUpgradeEffect)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public bool Success { get; init; }
    [FieldOrder(2)] public bool Cursed { get; init; }
    [FieldOrder(3)] public bool EnchantSkill { get; init; }
    [FieldOrder(4)] public int EnchantCategory { get; init; }
}
