using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities.Inventories;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserChangeSlotPositionRequest : StructuredRecvPacket
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    
    [FieldOrder(1)] public ItemInventoryType Type { get; init; }
    [FieldOrder(2)] public short OldPos { get; init; }
    [FieldOrder(3)] public short NewPos { get; init; }
    [FieldOrder(4)] public short Count { get; init; }
}
