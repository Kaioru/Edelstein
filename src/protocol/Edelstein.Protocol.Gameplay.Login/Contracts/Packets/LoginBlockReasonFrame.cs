using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets;

public record LoginBlockReasonFrame : StructuredBasePacket
{
    [FieldOrder(0)] public required byte Reason { get; init; }
    [FieldOrder(1)] public required FDateTime UnblockDate { get; init; }
}
