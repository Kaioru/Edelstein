using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record UserEnterField() : StructuredSendPacket((short)PacketSendOperation.UserEnterField)
{
    [FieldOrder(0)] public required int ObjectID { get; init; }
    
    [FieldOrder(1)] public byte Level { get; init; }
    [FieldOrder(2)] public required LPString CharacterName { get; init; }

    [FieldOrder(3)] public LPString GuildName { get; init; } = new();
    [FieldOrder(4)] public short GuildMarkBg { get; init; }
    [FieldOrder(5)] public byte GuildMarkBgColor { get; init; }
    [FieldOrder(6)] public short GuildMark { get; init; }
    [FieldOrder(7)] public byte GuildMarkColor { get; init; }
    
    [FieldOrder(8)] public long SecondaryStatFlag1 { get; init; }
    [FieldOrder(9)] public long SecondaryStatFlag2 { get; init; }
    [FieldOrder(10)] public byte SecondaryStatDefenseAtt { get; init; }
    [FieldOrder(11)] public byte SecondaryStatDefenseState { get; init; }
    
    [FieldOrder(12)] public short Job { get; init; }
    [FieldOrder(13)] public required StructuredCharacterLook CharacterLook { get; init; }
    
    [FieldOrder(14)] public int DriverID { get; init; }
    [FieldOrder(15)] public int PassengerID { get; init; }
    
    [FieldOrder(16)] public int ChocoCount { get; init; }
    [FieldOrder(17)] public int ActiveEffectItemID { get; init; }
    [FieldOrder(18)] public int CompletedSetItemID { get; init; }
    [FieldOrder(19)] public int PortableChairID { get; init; }
    
    [FieldOrder(20)] public short X { get; init; }
    [FieldOrder(21)] public short Y { get; init; }
    [FieldOrder(22)] public byte MoveAction { get; init; }
    [FieldOrder(23)] public short Foothold { get; init; }
    
    [FieldOrder(24)] public bool ShowAdminEffect { get; init; }
    
    [FieldOrder(25)] public bool HasActivePet { get; init; }
    
    [FieldOrder(26)] public int TamingMobLevel { get; init; }
    [FieldOrder(27)] public int TamingMobEXP { get; init; }
    [FieldOrder(28)] public int TamingMobFatigue { get; init; }
    
    [FieldOrder(29)] public bool HasMiniRoom { get; init; }
    [FieldOrder(30)] public bool HasAdBoard { get; init; }
    [FieldOrder(31)] public bool HasCoupleItem { get; init; }
    [FieldOrder(32)] public bool HasFriendshipItem { get; init; }
    [FieldOrder(33)] public bool HasMarriage { get; init; }
    
    [FieldOrder(34)] public byte DelayedEffectFlag { get; init; }
    
    [FieldOrder(35)] public bool HasNewYearCard { get; init; }
    
    [FieldOrder(36)] public int Phase { get; init; }
}
