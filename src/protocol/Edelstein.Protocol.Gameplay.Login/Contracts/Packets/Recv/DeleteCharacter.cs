using BinarySerialization;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Recv;

public record DeleteCharacter : StructuredRecvPacket
{
    [FieldOrder(0)]
    public required LPString SPW { get; init; }
    
    [FieldOrder(1)]
    public required int CharacterID { get; init; }
}
