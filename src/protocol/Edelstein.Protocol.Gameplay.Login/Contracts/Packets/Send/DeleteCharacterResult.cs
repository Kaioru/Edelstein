using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record DeleteCharacterResult() : StructuredSendPacket((short)PacketSendOperation.DeleteCharacterResult)
{
    [FieldOrder(0)]
    public required int CharacterID { get; init; }
    
    [FieldOrder(1)] 
    public required LoginSPWResultCode Result { get; init; }
}
