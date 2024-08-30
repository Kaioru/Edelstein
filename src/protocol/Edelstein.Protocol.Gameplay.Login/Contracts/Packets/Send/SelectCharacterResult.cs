using BinarySerialization;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record SelectCharacterResult() : StructuredSendPacket((short)PacketSendOperation.SelectCharacterResult)
{
    [FieldOrder(0)] public required LoginResultCode Result { get; init; }
    
    [FieldOrder(1)] public LoginAuthOption Option { get; init; }
    
    [FieldOrder(2)]
    [SerializeWhen(nameof(Result), LoginResultCode.Success)]
    public SelectCharacterResultInfo? Info { get; init; }
}

public record SelectCharacterResultInfo : StructuredBasePacket
{
    [FieldOrder(0)]
    [FieldCount(4)]
    public required byte[] Address { get; init; } = {0, 0, 0, 0};
    
    [FieldOrder(1)]
    public required short Port { get; init; }
    
    [FieldOrder(2)]
    public required int CharacterID { get; init; }
    
    [FieldOrder(3)]
    public byte AuthenCode { get; init; }
    
    [FieldOrder(4)]
    public int PremiumArgument { get; init; }
}
