using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserItemReleaseEffect() : StructuredSendPacket((short)PacketSendOperation.UserItemReleaseEffect)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required short Pos { get; init; }
}
