using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record TemporaryStatReset() : StructuredSendPacket((short)PacketSendOperation.TemporaryStatReset)
{
    [FieldOrder(0)]
    [FieldCount(4)]
    public required int[] Flag { get; init; }
    
    [FieldOrder(1)]
    public bool IsMovementAffectingStat { get; init; }
    
    [FieldOrder(2)] 
    [SerializeWhen(nameof(IsMovementAffectingStat), true)]
    public byte SN { get; init; }
}
