using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserMove() : StructuredSendPacket((short)PacketSendOperation.UserMove)
{
    [FieldOrder(0)]
    public required int ObjectID { get; init; }
    
    [FieldOrder(1)]
    public required StructuredMovePath Path { get; init; }
}
