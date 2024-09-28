using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record CONTIMOVE() : StructuredSendPacket((short)PacketSendOperation.CONTIMOVE)
{
    [FieldOrder(0)] public required ContiMoveTarget Target { get; init; }
    [FieldOrder(1)] public required ContiMoveStateTrigger Trigger { get; init; }
}
