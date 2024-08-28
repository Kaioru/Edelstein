using BinarySerialization;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record CreateNewCharacterResult() : StructuredSendPacket((short)PacketSendOperation.CreateNewCharacterResult)
{
    [FieldOrder(0)] 
    public required LoginResultCode Result { get; init; }
    
    [FieldOrder(1)]
    [SerializeWhen(nameof(Result), LoginResultCode.Success)]
    public CreateNewCharacterResultInfo? Info { get; init; }
}

public record CreateNewCharacterResultInfo : StructuredBasePacket
{
    [FieldOrder(0)]
    public required StructuredCharacterStat CharacterStat { get; init; }
    
    [FieldOrder(1)]
    public required StructuredAvatarLook AvatarLook { get; init; }
}
