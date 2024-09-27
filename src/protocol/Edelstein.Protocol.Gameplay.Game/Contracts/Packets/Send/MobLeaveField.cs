using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record MobLeaveField() : StructuredSendPacket((short)PacketSendOperation.MobLeaveField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required FieldMobLeaveType LeaveType { get; init; }
}
