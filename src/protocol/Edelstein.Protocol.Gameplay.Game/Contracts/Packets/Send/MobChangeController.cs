using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record MobChangeController() : StructuredSendPacket((short)PacketSendOperation.MobChangeController)
{
    [FieldOrder(0)] public required bool Controller { get; init; }
    [FieldOrder(1)] public required int ObjectID { get; init; }
    
    [FieldOrder(2)] 
    [SerializeWhen(nameof(Controller), true)]
    public StructuredMobInfo? Info { get; init; }
}
