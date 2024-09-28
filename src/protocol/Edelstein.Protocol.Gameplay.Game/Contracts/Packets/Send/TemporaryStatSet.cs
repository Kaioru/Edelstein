using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record TemporaryStatSet() : StructuredSendPacket((short)PacketSendOperation.TemporaryStatSet)
{
    [FieldOrder(0)]
    public required StructuredTemporaryStatsLocal Stats { get; init; }
    
    [FieldOrder(1)]
    public short Delay { get; init; }
    
    [FieldOrder(2)]
    public bool IsMovementAffectingStat { get; init; }
    
    [FieldOrder(3)] 
    [SerializeWhen(nameof(IsMovementAffectingStat), true)]
    public byte SN { get; init; }
}
