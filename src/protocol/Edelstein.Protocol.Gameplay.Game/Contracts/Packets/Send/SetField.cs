using BinarySerialization;
using Edelstein.Protocol.Gameplay.Entities;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;

namespace Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;

public record SetField() : StructuredSendPacket((short)PacketSendOperation.SetField)
{
    [FieldOrder(0)]
    public short ClientOptCount { get; init; }
    
    [FieldOrder(1)]
    public required int ChannelID { get; init; }
    
    [FieldOrder(2)]
    public int OldDriverID { get; init; }
    
    [FieldOrder(3)]
    public bool Unk1 { get; init; }
    
    [FieldOrder(4)]
    public bool IsInitialize { get; init; }
    
    [FieldOrder(5)]
    public short Unk2 { get; init; }
    
    [FieldOrder(6)]
    [Subtype(nameof(IsInitialize), true, typeof(SetFieldInfoCharacterInit))]
    [Subtype(nameof(IsInitialize), false, typeof(SetFieldInfoCharacter))]
    [SubtypeDefault(typeof(SetFieldInfoCharacter))]
    public SetFieldInfo? Info { get; init; }
    
    [FieldOrder(7)]
    public required FDateTime DateServer { get; init; }
}

public record SetFieldInfo : StructuredBasePacket;

public record SetFieldInfoCharacterInit : SetFieldInfo
{
    [FieldOrder(0)]
    public required uint Seed1 { get; init; }
    
    [FieldOrder(1)]
    public required uint Seed2 { get; init; }
    
    [FieldOrder(2)]
    public required uint Seed3 { get; init; }
    
    [FieldOrder(3)]
    public required StructuredCharacterData Data { get; init; }
    
    // LogoutGift
    [FieldOrder(4)]
    public int PredictQuit { get; init; }
    
    [FieldOrder(5)]
    [FieldCount(3)]
    public int[] LogoutGiftCommoditySN { get; init; } = {0, 0, 0, 0};
}

public record SetFieldInfoCharacter : SetFieldInfo
{
    [FieldOrder(0)]
    public bool Unk1 { get; init; }
    
    [FieldOrder(1)]
    public required int PosMap { get; init; }
    
    [FieldOrder(2)]
    public required byte Portal { get; init; }
    
    [FieldOrder(3)]
    public required int HP { get; init; }
    
    [FieldOrder(4)]
    public bool ChaseEnable { get; init; }
    
    [FieldOrder(5)]
    [SerializeWhen(nameof(ChaseEnable), true)]
    public SetFieldInfoCharacterChase? ChaseInfo { get; init; }
}

public record SetFieldInfoCharacterChase : StructuredBasePacket
{
    [FieldOrder(0)]
    public required int TargetPositionX { get; init; }
    
    [FieldOrder(1)]
    public required int TargetPositionY { get; init; }
}
