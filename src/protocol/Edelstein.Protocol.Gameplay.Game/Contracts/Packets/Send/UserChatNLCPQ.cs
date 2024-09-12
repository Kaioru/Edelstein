using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserChatNLCPQ() : StructuredSendPacket((short)PacketSendOperation.UserChatNLCPQ)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public bool IsAdminChat { get; init; }
    [FieldOrder(2)] public required LPString Text { get; init; }
    [FieldOrder(3)] public bool OnlyBalloon { get; init; }
    [FieldOrder(4)] public required LPString Name { get; init; }
}
