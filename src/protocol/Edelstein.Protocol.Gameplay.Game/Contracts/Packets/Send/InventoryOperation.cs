using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories.Modifiers;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record InventoryOperation() : StructuredSendPacket((short)PacketSendOperation.InventoryOperation)
{
    [FieldOrder(0)] public required bool ExclRequest { get; init; }
    [FieldOrder(1)] public required StructuredModifyInventoryOperations Operations { get; init; }
    [FieldOrder(2)] public byte SN { get; init; }
}
