using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserItemUnreleaseEffect() : StructuredSendPacket((short)PacketSendOperation.UserItemUnreleaseEffect)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required bool Success { get; init; }
}
