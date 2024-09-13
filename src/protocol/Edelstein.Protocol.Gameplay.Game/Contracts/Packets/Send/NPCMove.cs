using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record NPCMove() : StructuredSendPacket((short)PacketSendOperation.NpcMove)
{
    [FieldOrder(0)] public int ObjectID { get; init; }
    
    [FieldOrder(1)] public byte Action { get; init; }
    [FieldOrder(2)] public byte ChatIdx { get; init; }
    
    [FieldOrder(3)] public StructuredMovePath? Path { get; init; }
}
