using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Stats;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserTemporaryStatSet() : StructuredSendPacket((short)PacketSendOperation.UserTemporaryStatSet)
{
    [FieldOrder(0)]
    public required int ObjectID { get; init; }
    
    [FieldOrder(1)]
    public required StructuredTemporaryStatsRemote Stats { get; init; }
    
    [FieldOrder(2)]
    public short Delay { get; init; }
}
