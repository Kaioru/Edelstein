using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Stats.Modifiers;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record StatChanged() : StructuredSendPacket((short)PacketSendOperation.StatChanged)
{
    [FieldOrder(0)] public required bool ExclRequest { get; init; }
    [FieldOrder(1)] public required StructuredModifyStat Stats { get; init; }
    [FieldOrder(2)] public bool Unk1 { get; init; } // SN
    [FieldOrder(3)] public bool Unk2 { get; init; } // HPRecovery, MPRecovery
}
