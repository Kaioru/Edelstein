using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserChat() : StructuredSendPacket((short)PacketSendOperation.UserChat)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public bool IsAdminChat { get; init; }
    [FieldOrder(2)] public required LPString Text { get; init; }
    [FieldOrder(3)] public bool OnlyBalloon { get; init; }
}
