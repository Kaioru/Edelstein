using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record SelectWorld : StructuredRecvPacket
{
    [FieldOrder(0)] public required byte GameStartMode { get; init; }
    
    // TODO if (GameStartMode == 1) extra data
    
    [FieldOrder(1)] public required byte WorldID { get; init; }
    [FieldOrder(2)] public required byte ChannelID { get; init; }
    
    [FieldOrder(3)] 
    [FieldLength(0x4)]
    public required byte[] Address { get; init; }
}
