using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record MobEnterField() : StructuredSendPacket((short)PacketSendOperation.MobEnterField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required StructuredMobInfo Info { get; init; }
}
