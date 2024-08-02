using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Contracts.Packets.Recv;

public record UpdateScreenSetting : StructuredRecvPacket
{
    [FieldOrder(0)] public required bool IsLargeScreen { get; init; }
    [FieldOrder(1)] public required bool IsWindowedMode { get; init; }
}
