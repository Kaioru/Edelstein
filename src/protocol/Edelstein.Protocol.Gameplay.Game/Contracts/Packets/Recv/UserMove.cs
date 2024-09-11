using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Recv;

public record UserMove : StructuredRecvPacket
{
    [FieldOrder(0)] public int DR0 { get; init; }
    [FieldOrder(1)] public int DR1 { get; init; }
    
    [FieldOrder(2)] public byte FieldKey { get; init; }
    
    [FieldOrder(3)] public int DR2 { get; init; }
    [FieldOrder(4)] public int DR3 { get; init; }
    
    [FieldOrder(5)] public int Crc { get; init; }
    [FieldOrder(6)] public int Key { get; init; }
    [FieldOrder(7)] public int Crc32 { get; init; }

    [FieldOrder(8)] public StructuredMovePath Path { get; init; } = new();
}
