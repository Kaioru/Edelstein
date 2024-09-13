using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record NPCEnterField() : StructuredSendPacket((short)PacketSendOperation.NpcEnterField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required StructuredNPCInfo Info { get; init; }
}
