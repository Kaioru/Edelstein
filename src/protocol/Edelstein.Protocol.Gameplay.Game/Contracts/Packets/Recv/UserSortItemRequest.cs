using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserSortItemRequest : StructuredRecvPacket
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    [FieldOrder(1)] public ItemInventoryType Type { get; init; }
}
