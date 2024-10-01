using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Items;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserItemReleaseRequest : StructuredRecvPacket, IItemUseInfo
{
    [FieldOrder(0)] public int UpdateTime { get; init; }
    [FieldOrder(1)] public short UPOS { get; init; }
    [FieldOrder(2)] public short EPOS { get; init; }
    
    [Ignore] public short Pos => UPOS;
}
