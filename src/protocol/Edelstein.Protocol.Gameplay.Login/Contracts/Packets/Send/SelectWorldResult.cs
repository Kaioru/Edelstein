using System.Collections.Generic;
using BinarySerialization;
using Edelstein.Protocol.Gameplay.Contracts.Packets.Shared;
using Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Shared;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Protocol.Gameplay.Login.Contracts.Packets.Send;

public record SelectWorldResult() : StructuredSendPacket((short)PacketSendOperation.SelectWorldResult)
{
    [FieldOrder(0)] public required LoginResultCode Result { get; init; }
    
    [FieldOrder(1)]
    [SerializeWhen(nameof(Result), LoginResultCode.Success)]
    public SelectWorldResultSuccessInfo? Info { get; init; }
}

public record SelectWorldResultSuccessInfo : StructuredBasePacket
{
    [FieldOrder(0)]
    public byte CharacterCount { get; init; }

    [FieldOrder(1)] 
    [FieldCount(nameof(CharacterCount))]
    public List<SelectWorldResultSuccessInfoCharacter> Characters { get; init; } = new();
    
    [FieldOrder(2)] 
    public byte LoginOpt { get; init; }
    
    [FieldOrder(3)] 
    public int SlotCount { get; init; }
    
    [FieldOrder(4)] 
    public int BuyCharCount { get; init; }
}

public record SelectWorldResultSuccessInfoCharacter : StructuredBasePacket
{
    [FieldOrder(0)]
    public required StructuredCharacterStat CharacterStat { get; init; }
    
    [FieldOrder(1)]
    public required StructuredAvatarLook AvatarLook { get; init; }
    
    [FieldOrder(3)]
    public bool OnFamily { get; init; }
    
    [FieldOrder(4)]
    public bool IsRanked { get; init; }

    [FieldOrder(5)] 
    [SerializeWhen(nameof(IsRanked), true)]
    public StructuredRankInfo RankInfo { get; init; } = new();
}
