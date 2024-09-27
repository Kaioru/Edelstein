using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record MobCtrlAck() : StructuredSendPacket((short)PacketSendOperation.MobCtrlAck)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    [FieldOrder(1)] public required short MobCtrlSN { get; init; } 
    [FieldOrder(2)] public bool NextAttackPossible { get; init; }
    [FieldOrder(3)] public short MP { get; init; }
    [FieldOrder(4)] public byte SkillCommand { get; init; }
    [FieldOrder(5)] public byte SLV { get; init; }
}
