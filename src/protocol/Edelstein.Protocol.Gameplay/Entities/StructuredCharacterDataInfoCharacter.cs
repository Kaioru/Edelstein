using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Entities;

public record StructuredCharacterDataInfoCharacter : StructuredBasePacket
{
    [FieldOrder(0)]
    public required StructuredCharacterStat Stats { get; init; }
    
    [FieldOrder(1)]
    public required byte FriendMax { get; init; }
    
    [FieldOrder(2)]
    public bool Unk1 { get; init; }
}
