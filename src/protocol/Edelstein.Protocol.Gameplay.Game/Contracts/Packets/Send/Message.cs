using BinarySerialization;
using Edelstein.Protocol.Gameplay.Game.Objects.Users.Messages;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record Message() : StructuredSendPacket((short)PacketSendOperation.Message)
{
    [FieldOrder(0)]
    public required MessageType Type { get; init; }
    
    [FieldOrder(1)]
    [Subtype(nameof(Type), MessageType.SystemMessage, typeof(StructuredMessageInfoSystemMessage))]
    [SubtypeDefault(typeof(StructuredMessageInfo))]
    public required StructuredMessageInfo Info { get; init; }
}
