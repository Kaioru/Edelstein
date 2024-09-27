using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserEmotion : StructuredRecvPacket
{
    [FieldOrder(0)] public int Emotion { get; init; }
    [FieldOrder(1)] public int Duration { get; init; }
    [FieldOrder(2)] public bool ByItemOption { get; init; }
}
