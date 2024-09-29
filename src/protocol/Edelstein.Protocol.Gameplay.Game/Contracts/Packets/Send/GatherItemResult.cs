using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record GatherItemResult() : StructuredSendPacket((short)PacketSendOperation.GatherItemResult)
{
    [FieldOrder(0)] public byte Unk1 { get; init; }
    [FieldOrder(1)] public required ItemInventoryType Type { get; init; }
}
