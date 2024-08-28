using System.Runtime.InteropServices;
using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Shared;

public record StructuredRankInfo : StructuredBasePacket
{
    [FieldOrder(0)]
    public int WorldRank { get; init; }
    
    [FieldOrder(1)]
    public int WorldRankGap { get; init; }
    
    [FieldOrder(2)]
    public int JobRank { get; init; }
    
    [FieldOrder(3)]
    public int JobRankGap { get; init; }
}
