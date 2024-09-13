using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record NPCChangeController() : StructuredSendPacket((short)PacketSendOperation.NpcChangeController)
{
    [FieldOrder(0)] public required bool IsSetLocalNPC { get; init; }
    [FieldOrder(1)] public required int ObjectID { get; init; }
    
    [FieldOrder(2)] 
    [SerializeWhen(nameof(IsSetLocalNPC), true)]
    public StructuredNPCInfo? Info { get; init; }
}
