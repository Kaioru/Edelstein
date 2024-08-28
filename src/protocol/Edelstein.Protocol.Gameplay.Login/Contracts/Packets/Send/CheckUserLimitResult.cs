using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record CheckUserLimitResult() : StructuredSendPacket((short)PacketSendOperation.CheckUserLimitResult)
{
    [FieldOrder(0)] public byte OverUserLimit { get; init; }
    [FieldOrder(1)] public byte PopulateLevel { get; init; }
}
