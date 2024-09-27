using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserEmotion() : StructuredSendPacket((short)PacketSendOperation.UserEmotion)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public int Emotion { get; init; }
    [FieldOrder(2)] public int Duration { get; init; }
    [FieldOrder(3)] public bool ByItemOption { get; init; }
}
